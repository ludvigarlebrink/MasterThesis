using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MenuState
{
    Start,
    JoinHost,
    SelectRoom,
    CreateRoom,
    Room
}

public class MainMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private StartState m_StartState;
    [SerializeField] private JoinHostState m_JoinHostState;
    [SerializeField] private SelectRoomState m_SelectRoomState;
    [SerializeField] private CreateRoomState m_CreateRoomState;
    [SerializeField] private RoomState m_RoomState;

    private MenuState m_CurrentState = MenuState.Start;

    public void GoToState(MenuState state)
    {
        GetCurrentStateGameObject().SetActive(false);
        m_CurrentState = state;
        GetCurrentStateGameObject().SetActive(true);
    }

    private void Awake()
    {
        Screen.SetResolution(1080, 1920, false);
        PhotonNetwork.AutomaticallySyncScene = true;

        m_StartState.SetMainMenu(this);
        m_JoinHostState.SetMainMenu(this);
        m_SelectRoomState.SetMainMenu(this);
        m_CreateRoomState.SetMainMenu(this);
        m_RoomState.SetMainMenu(this);

        m_StartState.gameObject.SetActive(true);
        m_JoinHostState.gameObject.SetActive(false);
        m_SelectRoomState.gameObject.SetActive(false);
        m_CreateRoomState.gameObject.SetActive(false);
        m_RoomState.gameObject.SetActive(false);
    }

    private GameObject GetCurrentStateGameObject()
    {
        switch (m_CurrentState)
        {
            case MenuState.Start:
                return m_StartState.gameObject;
            case MenuState.JoinHost:
                return m_JoinHostState.gameObject;
            case MenuState.SelectRoom:
                return m_SelectRoomState.gameObject;
            case MenuState.CreateRoom:
                return m_CreateRoomState.gameObject;
            case MenuState.Room:
                return m_RoomState.gameObject;
            default:
                break;
        }

        return null;
    }

    private void Start()
    {
    }
}
