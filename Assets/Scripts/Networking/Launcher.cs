using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    private bool m_IsHost = false;

    private const string m_GameVersion = "1";

    public void CreateRoom(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(roomName, roomOptions);
        Debug.Log("Creating Room" + roomName);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // Critical! We failed to join a random room, maybe none exists or
        // they are all full. No worries, we create a new room.
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public void SetPlayerName(string value)
    {
        // Critical!
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }

        PhotonNetwork.NickName = value;
        PlayerPrefs.SetString(PlayerPrefKeys.playerName, value);
    }

    private void Awake()
    {
        // Critical!
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master
        // client and all clients in the same room sync their level automatically.
        PhotonNetwork.AutomaticallySyncScene = true;

        // Connect to master.
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = m_GameVersion;
    }
}
