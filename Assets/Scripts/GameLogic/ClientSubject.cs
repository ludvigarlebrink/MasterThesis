using Photon.Pun;
using UnityEngine;

public enum LevelIndex
{
    None,
    Bank,
    BlackMarket,
    Harbour,
    Jewlery,
    Market,
    Museum,
    Slum
}

[RequireComponent(typeof(PhotonView))]
public class ClientSubject : MonoBehaviour, IPunObservable
{
    public GameObject thiefObject = null;
    public LevelIndex levelIndex = LevelIndex.None;
    public GameObject clickFeedbackPrefab = null;

    private GameUI m_GameUI = null;
    private GameTimer m_GameTimer = null;
    private ScoreUI m_ScoreUI = null;

    private Thief m_Thief = null;
    private PathfindingAgent m_ThiefPathfindingAgent = null;
    private WorldManager m_WorldManager = null;
    private PhotonView m_PhotonView = null;
    private ParticleSystem m_ClickFeedback = null;

    // Added by Chris to deactivate loot after collection
    private Loot m_currentLootTarget = null;

    private bool m_IsMoving = false;
    private bool m_IsMovingTowardsExit = false;
    private bool m_IsMovingTowardsLootObject = false;
    private bool m_IsSetup = false;

    private bool m_IsBlocked = false;
    private float m_BlockedTimeCounter = 0.0f;
    private float m_BlockedTime = 4.0f;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            WriteStream(stream);
        }
        else
        {
            ReadStream(stream);
        }
    }

    private void OnReachedDestination()
    {
        if (m_IsMovingTowardsLootObject && m_currentLootTarget != null)
        {
            m_Thief.currentLoot += 1;
            m_Thief.currentNoise += 0.1f;
            m_IsMovingTowardsLootObject = false;
            // Collect loot
            m_currentLootTarget.Collect();
            m_currentLootTarget = null;
        }
        else if (m_IsMovingTowardsExit)
        {
            m_Thief.atExit = true;
            m_IsMovingTowardsExit = false;
            m_GameTimer.EndPrematurely();
        }

        if (m_IsMoving)
        {
            m_IsMoving = false;
        }
    }

    private void OnStartBankButton()
    {
        m_WorldManager.startBankButton.gameObject.SetActive(false);
        m_ThiefPathfindingAgent.transform.parent = m_WorldManager.levelBank.transform;
        m_ThiefPathfindingAgent.transform.localPosition = m_WorldManager.bankStartPosition.localPosition;
        m_ThiefPathfindingAgent.pathfindingManager = m_ThiefPathfindingAgent.GetComponentInParent<PathfindingManager>();
        levelIndex = LevelIndex.Bank;
        m_Thief.ResetThief();
        m_GameTimer.StartTimer(45.0f);
        m_Thief.gameObject.SetActive(true);
    }

    private void OnStartBlackMarketButton()
    {
        m_WorldManager.startBlackMarketButton.gameObject.SetActive(false);
        m_ThiefPathfindingAgent.transform.parent = m_WorldManager.levelBlackMarket.transform;
        m_ThiefPathfindingAgent.transform.localPosition = m_WorldManager.blackMarketStartPosition.localPosition;
        m_ThiefPathfindingAgent.pathfindingManager = m_ThiefPathfindingAgent.GetComponentInParent<PathfindingManager>();
        levelIndex = LevelIndex.BlackMarket;
        m_Thief.ResetThief();
        m_GameTimer.StartTimer(45.0f);
        m_Thief.gameObject.SetActive(true);
    }

    private void OnStartHarbourButton()
    {
        m_WorldManager.startHarbourButton.gameObject.SetActive(false);
        m_ThiefPathfindingAgent.transform.parent = m_WorldManager.levelHarbour.transform;
        m_ThiefPathfindingAgent.transform.localPosition = m_WorldManager.harbourStartPosition.localPosition;
        m_ThiefPathfindingAgent.pathfindingManager = m_ThiefPathfindingAgent.GetComponentInParent<PathfindingManager>();
        levelIndex = LevelIndex.Harbour;
        m_Thief.ResetThief();
        m_GameTimer.StartTimer(45.0f);
        m_Thief.gameObject.SetActive(true);
    }

    private void OnStartJewleryButton()
    {
        m_WorldManager.startJewleryButton.gameObject.SetActive(false);
        m_ThiefPathfindingAgent.transform.parent = m_WorldManager.levelJewlery.transform;
        m_ThiefPathfindingAgent.transform.localPosition = m_WorldManager.jewleryStartPosition.localPosition;
        m_ThiefPathfindingAgent.pathfindingManager = m_ThiefPathfindingAgent.GetComponentInParent<PathfindingManager>();
        levelIndex = LevelIndex.Jewlery;
        m_Thief.ResetThief();
        m_GameTimer.StartTimer(45.0f);
        m_Thief.gameObject.SetActive(true);
    }

    private void OnStartMarketButton()
    {
        m_WorldManager.startMarketButton.gameObject.SetActive(false);
        m_ThiefPathfindingAgent.transform.parent = m_WorldManager.levelMarket.transform;
        m_ThiefPathfindingAgent.transform.localPosition = m_WorldManager.marketStartPosition.localPosition;
        m_ThiefPathfindingAgent.pathfindingManager = m_ThiefPathfindingAgent.GetComponentInParent<PathfindingManager>();
        levelIndex = LevelIndex.Market;
        m_Thief.ResetThief();
        m_GameTimer.StartTimer(45.0f);
        m_Thief.gameObject.SetActive(true);
    }

    private void OnStartMuseumButton()
    {
        m_WorldManager.startMuseumButton.gameObject.SetActive(false);
        m_ThiefPathfindingAgent.transform.parent = m_WorldManager.levelMuseum.transform;
        m_ThiefPathfindingAgent.transform.localPosition = m_WorldManager.museumStartPosition.localPosition;
        m_ThiefPathfindingAgent.pathfindingManager = m_ThiefPathfindingAgent.GetComponentInParent<PathfindingManager>();
        levelIndex = LevelIndex.Museum;
        m_Thief.ResetThief();
        m_GameTimer.StartTimer(45.0f);
        m_Thief.gameObject.SetActive(true);
    }

    private void OnStartSlumButton()
    {
        m_WorldManager.startSlumButton.gameObject.SetActive(false);
        m_ThiefPathfindingAgent.transform.parent = m_WorldManager.levelSlum.transform;
        m_ThiefPathfindingAgent.transform.localPosition = m_WorldManager.slumStartPosition.localPosition;
        m_ThiefPathfindingAgent.pathfindingManager = m_ThiefPathfindingAgent.GetComponentInParent<PathfindingManager>();
        levelIndex = LevelIndex.Slum;
        m_Thief.ResetThief();
        m_GameTimer.StartTimer(45.0f);
        m_Thief.gameObject.SetActive(true);
    }

    private void OnTimerFinished()
    {
        switch (levelIndex)
        {
            case LevelIndex.None:
                break;
            case LevelIndex.Bank:
                m_WorldManager.startBankButton.gameObject.SetActive(true);
                break;
            case LevelIndex.BlackMarket:
                m_WorldManager.startBlackMarketButton.gameObject.SetActive(true);
                break;
            case LevelIndex.Harbour:
                m_WorldManager.startHarbourButton.gameObject.SetActive(true);
                break;
            case LevelIndex.Jewlery:
                m_WorldManager.startJewleryButton.gameObject.SetActive(true);
                break;
            case LevelIndex.Market:
                m_WorldManager.startMarketButton.gameObject.SetActive(true);
                break;
            case LevelIndex.Museum:
                m_WorldManager.startMuseumButton.gameObject.SetActive(true);
                break;
            case LevelIndex.Slum:
                m_WorldManager.startSlumButton.gameObject.SetActive(true);
                break;
            default:
                break;
        }

        levelIndex = LevelIndex.None;
        m_ScoreUI.Show(m_Thief.currentLoot, m_Thief.atExit);
        m_Thief.ResetThief();
        m_Thief.gameObject.SetActive(false);
        m_Thief.transform.parent = null;
    }

    private void ReadStream(PhotonStream stream)
    {
        LevelIndex state = (LevelIndex)stream.ReceiveNext();
        if (state != levelIndex)
        {
            if (thiefObject)
            {
                if (state == LevelIndex.None)
                {
                    thiefObject.SetActive(false);

                    // Set it in root node.
                    thiefObject.transform.parent = null;
                }
                else
                {
                    Level obj = m_WorldManager.GetLevel(state);

                    if (obj)
                    {
                        thiefObject.SetActive(true);
                        thiefObject.transform.parent = obj.transform;
                    }
                }

                levelIndex = state;
            }
        }
    }

    private void Start()
    {
        m_WorldManager = FindObjectOfType<WorldManager>();
        if (!m_WorldManager)
        {
            Debug.LogError("No WorldManager in scene!");
            return;
        }

        m_PhotonView = GetComponent<PhotonView>();
        m_ClickFeedback = Instantiate(clickFeedbackPrefab).GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (m_IsBlocked && m_BlockedTimeCounter <= 0)
        {
            m_IsBlocked = false;
            m_Thief.Stun(false);
        }

        if (!thiefObject)
        {
            SetupThief();
            return;
        }

        if (!m_PhotonView.IsMine)
        {
            return;
        }

        if (!m_IsSetup)
        {
            Setup();
        }

        // Continuous noise change
        if (m_Thief.currentNoise > 0)
        {
            m_Thief.currentNoise -= 0.1f * Time.deltaTime;
        }

        if (m_Thief.currentNoise < 1 && m_IsMoving && !m_IsBlocked)
        {
            m_Thief.currentNoise += 0.18f * Time.deltaTime;
        }

        m_Thief.currentNoise = Mathf.Clamp01(m_Thief.currentNoise);

        if (m_IsBlocked)
        {
            m_BlockedTimeCounter -= Time.deltaTime;
            return;
        }

        if (m_Thief.currentNoise == 1)
        {
            m_IsMoving = false;
            m_IsMovingTowardsLootObject = false;
            m_ThiefPathfindingAgent.SetDestination(thiefObject.transform.position);
            m_IsBlocked = true;
            m_Thief.Stun(true);
            m_BlockedTimeCounter = m_BlockedTime;
            return;
        }

        if (levelIndex != LevelIndex.None)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
                {
                    if (hit.collider.tag == "Ground")
                    {
                        m_IsMovingTowardsLootObject = false;
                        m_IsMovingTowardsExit = false;
                        m_IsMoving = true;
                        m_Thief.atExit = false;
                        m_IsMoving = m_ThiefPathfindingAgent.SetDestination(hit.point, true);
                    }
                    else if (hit.collider.tag == "Exit")
                    {
                        m_IsMovingTowardsLootObject = false;
                        m_IsMovingTowardsExit = true;
                        m_IsMoving = true;
                        m_IsMoving = m_ThiefPathfindingAgent.SetDestination(hit.point, true);
                    }
                    else if (hit.collider.GetComponent<Loot>())
                    {
                        Loot loot = hit.collider.GetComponent<Loot>();
                        if (loot.Collectable)
                        {
                            Debug.Log("Object : " + loot.GetPosition().ToString());
                            m_IsMoving = true;
                            m_IsMovingTowardsExit = true;
                            m_IsMovingTowardsLootObject = true;
                            m_Thief.atExit = false;
                            m_currentLootTarget = loot;
                            m_ThiefPathfindingAgent.SetDestination(loot.GetPosition());
                        }
                    }
                    
                    if (m_ClickFeedback.isPlaying)
                    {
                        m_ClickFeedback.Clear();
                    }

                    m_ClickFeedback.transform.position = hit.point;
                    m_ClickFeedback.Play();
                }
            }
        }
    }

    private void Setup()
    {
        if (m_WorldManager.startBankButton != null)
        {
            m_WorldManager.startBankButton.onClick.AddListener(OnStartBankButton);
        }

        if (m_WorldManager.startBlackMarketButton != null)
        {
            m_WorldManager.startBlackMarketButton.onClick.AddListener(OnStartBlackMarketButton);
        }

        if (m_WorldManager.startHarbourButton != null)
        {
            m_WorldManager.startHarbourButton.onClick.AddListener(OnStartHarbourButton);
        }

        if (m_WorldManager.startJewleryButton != null)
        {
            m_WorldManager.startJewleryButton.onClick.AddListener(OnStartJewleryButton);
        }

        if (m_WorldManager.startMarketButton != null)
        {
            m_WorldManager.startMarketButton.onClick.AddListener(OnStartMarketButton);
        }

        if (m_WorldManager.startMuseumButton != null)
        {
            m_WorldManager.startMuseumButton.onClick.AddListener(OnStartMuseumButton);
        }

        if (m_WorldManager.startSlumButton != null)
        {
            m_WorldManager.startSlumButton.onClick.AddListener(OnStartSlumButton);
        }

        m_GameTimer = GetComponent<GameTimer>();
        m_GameUI = FindObjectOfType<GameUI>();
        m_ScoreUI = Resources.FindObjectsOfTypeAll<ScoreUI>()[0];

        m_GameUI.gameTimer = m_GameTimer;

        m_GameTimer.onTimerFinished.AddListener(OnTimerFinished);

        m_IsSetup = true;
    }

    private void SetupThief()
    {
        foreach (Thief t in FindObjectsOfType<Thief>())
        {
            PhotonView photonView = t.GetComponent<PhotonView>();
            if (photonView && m_PhotonView.OwnerActorNr == photonView.OwnerActorNr)
            {
                m_Thief = t;
                thiefObject = t.gameObject;
                m_ThiefPathfindingAgent = thiefObject.GetComponent<PathfindingAgent>();
                m_ThiefPathfindingAgent.onReachedDestination.AddListener(OnReachedDestination);
                m_Thief.gameObject.SetActive(false);

                if (!m_ThiefPathfindingAgent)
                {
                    Debug.LogError("No pathfinding agent found in thief object.");
                }
            }
        }
    }

    private void WriteStream(PhotonStream stream)
    {
        stream.SendNext(levelIndex);
    }
}
