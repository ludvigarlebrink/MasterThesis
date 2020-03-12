using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomState : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text m_Player1NameText = null;
    [SerializeField] private Text m_Player2NameText = null;
    [SerializeField] private Text m_Player3NameText = null;
    [SerializeField] private Text m_Player4NameText = null;
    [SerializeField] private Button m_StartGameButton = null;
    [SerializeField] private Button m_BackButton = null;
    [SerializeField] private string m_LevelToLoad = "";

    private Dictionary<string, Player> m_Players = null;
    private MainMenu m_MainMenu = null;

    public override void OnDisable()
    {
        base.OnDisable();
        m_BackButton.onClick.RemoveListener(OnBackClicked);
        if (PhotonNetwork.IsMasterClient)
        {
            m_StartGameButton.onClick.RemoveListener(OnStartGameClicked);
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        if (PhotonNetwork.IsMasterClient)
        {
            m_StartGameButton.gameObject.SetActive(true);
            m_StartGameButton.onClick.AddListener(OnStartGameClicked);
            m_Player1NameText.text = PhotonNetwork.NickName + " (Host)";
        }
        else
        {
            m_StartGameButton.gameObject.SetActive(false);
            m_Player1NameText.text = PhotonNetwork.NickName;
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            m_Players.Add(player.NickName, player);
        }
        Refresh();

        m_BackButton.onClick.AddListener(OnBackClicked);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        m_Players[newPlayer.NickName] = newPlayer;
        Refresh();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        m_Players.Remove(otherPlayer.NickName);
        Refresh();
    }

    public void SetMainMenu(MainMenu mainMenu)
    {
        m_MainMenu = mainMenu;
    }

    private void Awake()
    {
        m_Players = new Dictionary<string, Player>();
    }

    private void OnBackClicked()
    {
        m_MainMenu.GoToState(MenuState.JoinHost);
    }

    private void OnStartGameClicked()
    {
        PhotonNetwork.LoadLevel(m_LevelToLoad);
    }

    private void Refresh()
    {
        m_Player1NameText.text = "NONE";
        m_Player2NameText.text = "NONE";
        m_Player3NameText.text = "NONE";
        m_Player4NameText.text = "NONE";

        int offset = 0;
        foreach (KeyValuePair<string, Player> player in m_Players)
        {
            switch (offset)
            {
                case 0:
                    if (player.Value.IsMasterClient)
                    {
                        m_Player1NameText.text = player.Key + " (Host)";
                    }
                    else
                    {
                        m_Player1NameText.text = player.Key;
                    }
                    break;
                case 1:
                    if (player.Value.IsMasterClient)
                    {
                        m_Player2NameText.text = player.Key + " (Host)";
                    }
                    else
                    {
                        m_Player2NameText.text = player.Key;
                    }
                    break;
                case 2:
                    if (player.Value.IsMasterClient)
                    {
                        m_Player3NameText.text = player.Key + " (Host)";
                    }
                    else
                    {
                        m_Player3NameText.text = player.Key;
                    }
                    break;
                case 3:
                    if (player.Value.IsMasterClient)
                    {
                        m_Player4NameText.text = player.Key + " (Host)";
                    }
                    else
                    {
                        m_Player4NameText.text = player.Key;
                    }
                    break;
                default:
                    break;
            }

            ++offset;
        }
    }
}
