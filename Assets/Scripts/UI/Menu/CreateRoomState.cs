using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomState : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField m_InputField;
    [SerializeField] private Button m_CreateRoomButton;
    [SerializeField] private Button m_BackButton;
    [SerializeField] private GameObject m_Connecting;

    [SerializeField] private int m_MinNameCharacters = 3;

    private CanvasGroup m_CanvasGroup = null;
    private MainMenu m_MainMenu = null;

    public void SetMainMenu(MainMenu mainMenu)
    {
        m_MainMenu = mainMenu;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        m_InputField.onValueChanged.RemoveListener(OnInputChanged);
        m_CreateRoomButton.onClick.RemoveListener(OnCreateRoomClicked);
        m_BackButton.onClick.RemoveListener(OnBackClicked);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        m_InputField.onValueChanged.AddListener(OnInputChanged);
        m_CreateRoomButton.onClick.AddListener(OnCreateRoomClicked);
        m_BackButton.onClick.AddListener(OnBackClicked);
    }

    public override void OnCreatedRoom()
    {
        m_CanvasGroup.interactable = true;
        m_Connecting.SetActive(false);
        m_MainMenu.GoToState(MenuState.Room);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        m_CanvasGroup.interactable = true;
        m_Connecting.SetActive(false);
    }

    private void OnBackClicked()
    {
        m_MainMenu.GoToState(MenuState.JoinHost);
    }

    private void OnCreateRoomClicked()
    {
        m_CanvasGroup.interactable = false;
        m_Connecting.SetActive(true);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(m_InputField.text, roomOptions);
    }

    private void OnInputChanged(string value)
    {
        m_CreateRoomButton.interactable = m_InputField.text.Length >= m_MinNameCharacters;
    }

    private void Start()
    {
        m_CreateRoomButton.interactable = m_InputField.text.Length >= m_MinNameCharacters;
        m_CanvasGroup = GetComponent<CanvasGroup>();
    }
}
