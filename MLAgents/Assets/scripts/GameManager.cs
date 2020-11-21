using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    [Tooltip("The prefabs to use for representing the player")]
    public GameObject[] playerPrefabs;

    void Awake()
    {
        //this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
    }

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;

            if (drive.LocalPlayerInstance == null)
            {
                Debug.LogFormat("Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                int colour = PlayerPrefs.GetInt("Colour");
                PhotonNetwork.Instantiate(this.playerPrefabs[colour].name, new Vector3(0f, 1f, 0f), Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);

        if(PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() IsMasterClient {0}", PhotonNetwork.IsMasterClient);
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);

        if(PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom() IsMasterClient {0}", PhotonNetwork.IsMasterClient);
        }
    }

    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void CheckForWin(int lap, bool isPlayer)
    {
        // definitely needs refining, at this point you could just go backwards twice and win lol
        if(isPlayer && lap > 3)
        {
            Debug.Log("Player Won");
        }
        else if(!isPlayer && lap > 3)
        {
            Debug.Log("Agent Won");
        }
    }
}
