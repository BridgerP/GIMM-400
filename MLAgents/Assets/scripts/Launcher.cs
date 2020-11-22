using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        /// </summary>
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 3;

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;
        [Tooltip("The UI Label to inform the user chosen colour")]
        [SerializeField]
        private Text colourLabel;

        #endregion

        #region Private Fields
        ///<summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        ///</summary>
        string gameVersion = "1";

        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>
        bool isConnecting;
        private bool pressedSolo = false;
        
        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            PlayerPrefs.SetInt("Colour", 0);
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if(PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        public void PlaySolo()
        {
            pressedSolo = true;
            Connect();
        }

        public void PickColour(int col)
        {
            PlayerPrefs.SetInt("Colour", col);
            switch(col)
            {
                case 0:
                    colourLabel.text = "Blue";
                    break;
                case 1:
                    colourLabel.text = "Pink";
                    break;
                case 2:
                    colourLabel.text = "Purple";
                    break;
                case 3:
                    colourLabel.text = "Orange";
                    break;
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster was called by PUN");
            // #Critical: The first thing we try to do is join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            if(isConnecting)
            {
                isConnecting = false;
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            isConnecting = false;
            Debug.LogWarningFormat("OnDisconnected was called by PUN");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed was called by Pun. Calling Create Room.");

            if(pressedSolo)
            {
                PhotonNetwork.CreateRoom(null, new RoomOptions{ MaxPlayers = 1 });
            }
            else
            {
                // #Critical: We failed to join a random room, so we'll create a new one
                PhotonNetwork.CreateRoom(null, new RoomOptions{ MaxPlayers = maxPlayersPerRoom});
            }
        }

        public override void OnJoinedRoom()
        {
            // #Critical: We only load if we are the first player
            if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                if(pressedSolo)
                {
                    PlayerPrefs.SetInt("IsSolo", 1);
                    PhotonNetwork.LoadLevel("ML Assets");
                }
                else
                {
                    PlayerPrefs.SetInt("IsSolo", 0);
                    // #Critical: Load Room Level
                    PhotonNetwork.LoadLevel("MLMulti");
                }
            }
            Debug.Log("OnJoinedRoom was called by PUN. We are now in a room.");
        }

        #endregion
    }
