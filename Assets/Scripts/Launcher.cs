using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    [Tooltip("The maximum number of players per room. When a room is full," +
        " it can't be joined by new players, and so new room will be created")]
    [SerializeField] private byte m_MaxPlayersPerRoom = 6;
    private string m_GameVersion = "1";

    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called" +
            " by PUN");

        // Critical! The first we try to do is to join a potential existing room. 
        // If there is, good, else, we'll be called back with OnJoinRandomFailed().
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected()" +
            " was called by PUN with reason {0}", cause);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called" +
            " by PUN. No random room available, so we create one.\nCalling:" +
            "PhotonNetwork.CreateRoom");

        // Critical! We failed to join a random room, maybe none exists or
        // they are all full. No worries, we create a new room.
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = m_MaxPlayersPerRoom;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }


    private void Awake()
    {
        // Critical!
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master
        // client and all clients in the same room sync their level automatically.
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Connect()
    {
        // We check if we are connected or not, we join if we are,
        // else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // Critical! We need at this point to attempt joining a Random Room. If it fails,
            // we will get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // Critical! We must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = m_GameVersion;
        }
    }

    private void Start()
    {
        Connect();
    }
}
