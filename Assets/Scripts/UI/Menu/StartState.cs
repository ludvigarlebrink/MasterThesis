using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartState : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField m_InputField = null;
    [SerializeField] private Button m_NextButton = null;
    [SerializeField] private Text m_InfoText = null;
    [SerializeField] private GameObject m_Connecting = null;

    [SerializeField] private int m_MinNameCharacters = 3;

    private CanvasGroup m_CanvasGroup = null;
    private MainMenu m_MainMenu = null;

    public override void OnConnectedToMaster()
    {
        m_Connecting.SetActive(false);
        m_CanvasGroup.interactable = true;
        m_MainMenu.GoToState(MenuState.JoinHost);
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
    }

    public override void OnDisable()
    {
        base.OnDisable();
        m_NextButton.onClick.RemoveListener(OnNextClicked);
        m_InputField.onValueChanged.RemoveListener(OnTextFieldInput);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        m_NextButton.onClick.AddListener(OnNextClicked);
        m_InputField.onValueChanged.AddListener(OnTextFieldInput);
    }

    public void SetMainMenu(MainMenu mainMenu)
    {
        m_MainMenu = mainMenu;
    }

    private void OnNextClicked()
    {
        m_CanvasGroup.interactable = false;
        m_Connecting.SetActive(true);

        PhotonNetwork.NickName = m_InputField.text;
        PlayerPrefs.SetString(PlayerPrefKeys.playerName, m_InputField.text);

        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "0.0.1";
        Debug.Log("Connecting to Master");
    }

    private void OnTextFieldInput(string value)
    {
        m_NextButton.interactable = m_InputField.text.Length >= m_MinNameCharacters;
        RefreshWarnings();
    }

    private void RefreshWarnings()
    {
        m_InfoText.text = "";
        if (m_InputField.text.Length < m_MinNameCharacters)
        {
            m_InfoText.text = "* Name must consist of 3 to 16 characters.";
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(PlayerPrefKeys.playerName))
        {
            m_InputField.text = PlayerPrefs.GetString(PlayerPrefKeys.playerName);
        }

        m_NextButton.interactable = m_InputField.text.Length >= m_MinNameCharacters;
        m_CanvasGroup = GetComponent<CanvasGroup>();
        RefreshWarnings();
    }
}
