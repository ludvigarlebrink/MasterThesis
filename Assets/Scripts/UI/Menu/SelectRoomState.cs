using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectRoomState : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button m_BackButton = null;
    [SerializeField] private GameObject m_TheThing = null;
    [SerializeField] private GameObject m_RoomPrefab = null;
    [SerializeField] private GameObject m_Connecting = null;

    private Dictionary<string, RoomInfo> m_RoomInfos = null;
    private List<Room> m_Rooms = null;
    private CanvasGroup m_CanvasGroup = null;
    private MainMenu m_MainMenu = null;

    public override void OnDisable()
    {
        base.OnDisable();
        m_BackButton.onClick.RemoveListener(OnBackClicked);
        Clear();
        m_RoomInfos.Clear();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        m_BackButton.onClick.AddListener(OnBackClicked);
        PhotonNetwork.JoinLobby();
        m_TheThing.SetActive(false);
    }

    public override void OnJoinedLobby()
    {
        m_TheThing.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        m_CanvasGroup.interactable = true;
        m_Connecting.SetActive(false);
        m_MainMenu.GoToState(MenuState.Room);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        m_CanvasGroup.interactable = true;
        m_Connecting.SetActive(false);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
            {
                m_RoomInfos.Remove(roomInfo.Name);
                continue;
            }

            if (!roomInfo.IsOpen)
            {
                m_RoomInfos.Remove(roomInfo.Name);
                continue;
            }

            if (!roomInfo.IsVisible)
            {
                continue;
            }

            m_RoomInfos[roomInfo.Name] = roomInfo;
        }

        Refresh();
    }

    public void SetMainMenu(MainMenu mainMenu)
    {
        m_MainMenu = mainMenu;
    }

    private void Awake()
    {
        m_Rooms = new List<Room>();
        m_RoomInfos = new Dictionary<string, RoomInfo>();
        m_CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void Clear()
    {
        foreach (Room room in m_Rooms)
        {
            m_BackButton.onClick.AddListener(OnBackClicked);
            Destroy(room.gameObject);
        }

        m_Rooms.Clear();
    }

    private void OnBackClicked()
    {
        m_MainMenu.GoToState(MenuState.JoinHost);
    }

    private void OnJoinClicked(Room room)
    {
        m_CanvasGroup.interactable = false;
        m_Connecting.SetActive(true);
        PhotonNetwork.JoinRoom(room.roomName.text);
    }

    private void Refresh()
    {
        Clear();

        int offset = 0;
        foreach (KeyValuePair<string, RoomInfo> roomInfo in m_RoomInfos)
        {
            GameObject roomObject = Instantiate(m_RoomPrefab);
            RectTransform rectTransform = roomObject.GetComponent<RectTransform>();
            rectTransform.SetParent(m_TheThing.transform, false);
            rectTransform.anchorMin = new Vector2(0.0f, 1.0f);
            rectTransform.anchorMax = new Vector2(1.0f, 1.0f);
            rectTransform.pivot = new Vector2(0.5f, 1.0f);
            rectTransform.SetLeft(10);
            rectTransform.SetRight(10);
            rectTransform.anchoredPosition = new Vector2(rectTransform.localPosition.x, -110.0f * offset - 10.0f);

            Room room = roomObject.GetComponent<Room>();
            room.roomName.text = roomInfo.Value.Name;
            room.numberOfPlayers.text = roomInfo.Value.PlayerCount.ToString() + "/4";
            room.joinButton.onClick.AddListener(() => { OnJoinClicked(room); });

            m_Rooms.Add(room);

            ++offset;
        }
    }
}
