﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class drive : MonoBehaviourPunCallbacks, IPunObservable
{

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;
    Rigidbody rb;

    public float brakeSpeed;
    public float maxSpeed;
    public float speed;
    public float rotSpeed;

    public int lap = 1;

    float velocity;
    float rotation;

    void Awake()
    {
        // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                drive.LocalPlayerInstance = this.gameObject;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "StartBox")
        {
            lap++;
            GameManager.Instance.CheckForWin(lap, true);
        }
    }
    void FixedUpdate()
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
        if (velocity - Mathf.Abs(rb.velocity.magnitude) > 1) velocity = Mathf.Abs((velocity + rb.velocity.magnitude) / 2);

        rb.velocity = transform.forward * velocity;
        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(stream.IsWriting)
            {
                // we own this player; send others our data
                // stream.SendNext(IsFiring);
                // stream.SendNext(Health);
            }
            else
            {
                // network player, receive data
                // this.IsFiring = (bool)stream.ReceiveNext();
                // this.Health = (float)stream.ReceiveNext();
            }
        }
}
