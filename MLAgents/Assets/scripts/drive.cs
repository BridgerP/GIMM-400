﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using UnityEngine.UI;

public class drive : MonoBehaviourPunCallbacks, IPunObservable
{

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;
    Rigidbody rb;
    public Text timerText;

    public float brakeSpeed;
    public float maxSpeed;
    public float speed;
    public float rotSpeed;

    public GameObject halfTrig;
    public GameObject lapTrig;
    public GameObject raceFinish;
    public GameObject WinScreen;
    public GameObject LoseScreen;

    public int lap = 1;
    public float[] startingPoints = new float[] {5.64f,2.46f,-0.64f,3.73f};

    float velocity;
    float rotation;
    private int count;
    public bool canDrive {get; private set;}
    private bool timerStarted = false;
    private bool hasLost;

    void Awake()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            drive.LocalPlayerInstance = this.gameObject;
        }
        count = PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.Log(count);
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
        
    }
    void Start()
    {
        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();

        if(_cameraWork != null)
        {
            if(photonView.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        }

        if(photonView.IsMine)
        {
            rb = GetComponent<Rigidbody>();
            //rb.isKinematic = true;
            this.transform.position = new Vector3(-6.5f, 1f, startingPoints[count - 1]);
            rotation = 90;
            this.transform.rotation = Quaternion.Euler(0f,90f,0f);
        }

        halfTrig = GameObject.Find("HalfpointTrigger"); // use this to connect all the pieces
        lapTrig = GameObject.Find("LapCompleteTrigger"); // use this to connect all the pieces
        lapTrig.SetActive(false);
        raceFinish = GameObject.Find("RaceFinishTrigger"); // use this to connect all the pieces
        raceFinish.SetActive(false);
        WinScreen = GameObject.Find("WinScreen"); // use this to connect all the pieces
        WinScreen.SetActive(false);
        LoseScreen = GameObject.Find("LoseScreen"); // use this to connect all the pieces
        LoseScreen.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        // if(other.tag == "StartBox" && photonView.IsMine)
        // {
        //     lap++;
        //     GameManager.Instance.CheckForWin(lap, true);
        // }

        if(other.tag == "halfpoint" && photonView.IsMine)
        {
            lapTrig.SetActive(true);
            halfTrig.SetActive(false);
        }

        if(other.tag == "lapcomplete" && photonView.IsMine)
        {
            LapComplete.instance.callTrigger();
        }

        if(other.tag == "racefinish" && photonView.IsMine)
        {
            WinScreen.SetActive(true);
            canDrive = false;
        }
    }

    void Update()
    {
        if(photonView.IsMine && canDrive)
        {
            if (Input.GetKey(KeyCode.S) && velocity > 10) velocity /= 1 + brakeSpeed;
            else if (Input.GetKey(KeyCode.S) && velocity > 2) velocity /= 1 + brakeSpeed * 1.5f;
            else if (Input.GetKey(KeyCode.S) && velocity > .1) velocity /= 1 + brakeSpeed * 2;
            else if (Input.GetKey(KeyCode.S)) velocity = -5f;
            else if (Input.GetKey(KeyCode.W))
            {
                if (Mathf.Abs(rb.velocity.z) < maxSpeed)
                    velocity += speed;
            }
            else velocity /= 1 + brakeSpeed / 4;

            if (velocity > 20)
            {
                if (Input.GetKey(KeyCode.A)) rotation -= rotSpeed / (Mathf.Abs(velocity) / 15);
                if (Input.GetKey(KeyCode.D)) rotation += rotSpeed / (Mathf.Abs(velocity) / 15);
            }
            else if (velocity > 7)
            {
                if (Input.GetKey(KeyCode.A)) rotation -= rotSpeed / (Mathf.Abs(velocity) / 5);
                if (Input.GetKey(KeyCode.D)) rotation += rotSpeed / (Mathf.Abs(velocity) / 5);
            }
            else if (velocity != 0)
            {
                if (Input.GetKey(KeyCode.A)) rotation -= 1;
                if (Input.GetKey(KeyCode.D)) rotation += 1;
            }
            else if (velocity < 0)
            {
                if (Input.GetKey(KeyCode.A)) rotation += 1;
                if (Input.GetKey(KeyCode.D)) rotation -= 1;
            }

            if (velocity > maxSpeed) velocity = 38;
            //if (velocity - Mathf.Abs(rb.velocity.magnitude) > 1) velocity = Mathf.Abs((velocity + rb.velocity.magnitude) / 2);

            rb.velocity = (transform.forward * velocity) - transform.up;
            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
        if(!timerStarted && photonView.IsMine)
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount == 3)
            {
                timerStarted = true;
                StartCoroutine("Timer");
            }
        }
        if(photonView.IsMine && hasLost)
        {
            canDrive = false;
            LoseScreen.SetActive(true);
        }
    }

    private IEnumerator Timer()
    {
        timerText.text = "3";
        yield return new WaitForSeconds(1.0f);
        timerText.text = "2";
        yield return new WaitForSeconds(1.0f);
        timerText.text = "1";
        yield return new WaitForSeconds(1.0f);
        timerText.text = "GO!";
        AudioManager.Instance.PlayStartSound();
        canDrive = true;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            // we own this player; send others our data
            // stream.SendNext(IsFiring);
            stream.SendNext(LapComplete.instance.LapsDone); // send to other players what lap I'm on
        }
        else
        {
            // network player, receive data
            // this.IsFiring = (bool)stream.ReceiveNext();
            this.hasLost = (int)stream.ReceiveNext() >= 2; // if another player has done enough laps, I've lost
        }
    }
}
