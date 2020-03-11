using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinHostState : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button m_JoinButton;
    [SerializeField] private Button m_HostButton;
    [SerializeField] private Button m_BackButton;

    private MainMenu m_MainMenu = null;

    public void SetMainMenu(MainMenu mainMenu)
    {
        m_MainMenu = mainMenu;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        m_JoinButton.onClick.RemoveListener(OnJoinClicked);
        m_HostButton.onClick.RemoveListener(OnHostClicked);
        m_BackButton.onClick.RemoveListener(OnBackClicked);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        m_JoinButton.onClick.AddListener(OnJoinClicked);
        m_HostButton.onClick.AddListener(OnHostClicked);
        m_BackButton.onClick.AddListener(OnBackClicked);
    }

    private void OnBackClicked()
    {
        PhotonNetwork.Disconnect();
        m_MainMenu.GoToState(MenuState.Start);
    }

    private void OnHostClicked()
    {
        m_MainMenu.GoToState(MenuState.CreateRoom);

    }

    private void OnJoinClicked()
    {
        m_MainMenu.GoToState(MenuState.SelectRoom);
    }
}
