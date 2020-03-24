using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
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
    public GameObject thief = null;
    public LevelIndex levelIndex = LevelIndex.None;

    private PathfindingAgent m_ThiefPathfindingAgent;
    private WorldManager m_WorldManager = null;
    private PhotonView m_PhotonView = null;

    private bool m_IsSetup = false;

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

    private void OnStartBankButton()
    {
        m_WorldManager.startBankButton.gameObject.SetActive(false);
        m_ThiefPathfindingAgent.transform.parent = m_WorldManager.levelBank.transform;
        m_ThiefPathfindingAgent.transform.localPosition = m_WorldManager.bankStartPosition.localPosition;
        m_ThiefPathfindingAgent.pathfindingManager = m_ThiefPathfindingAgent.GetComponentInParent<PathfindingManager>();
        levelIndex = LevelIndex.Bank;
    }

    private void OnStartBlackMarketButton()
    {
        m_WorldManager.startBlackMarketButton.gameObject.SetActive(false);
        m_ThiefPathfindingAgent.transform.parent = m_WorldManager.levelBlackMarket.transform;
        m_ThiefPathfindingAgent.transform.localPosition = m_WorldManager.blackMarketStartPosition.localPosition;
        m_ThiefPathfindingAgent.pathfindingManager = m_ThiefPathfindingAgent.GetComponentInParent<PathfindingManager>();
        levelIndex = LevelIndex.BlackMarket;
    }

    private void OnStartHarbourButton()
    {
        m_WorldManager.startHarbourButton.gameObject.SetActive(false);
        m_ThiefPathfindingAgent.transform.parent = m_WorldManager.levelHarbour.transform;
        m_ThiefPathfindingAgent.transform.localPosition = m_WorldManager.harbourStartPosition.localPosition;
        m_ThiefPathfindingAgent.pathfindingManager = m_ThiefPathfindingAgent.GetComponentInParent<PathfindingManager>();
        levelIndex = LevelIndex.Harbour;
    }

    private void OnStartJewleryButton()
    {
        m_WorldManager.startJewleryButton.gameObject.SetActive(false);
        m_ThiefPathfindingAgent.transform.parent = m_WorldManager.levelJewlery.transform;
        m_ThiefPathfindingAgent.transform.localPosition = m_WorldManager.jewleryStartPosition.localPosition;
        m_ThiefPathfindingAgent.pathfindingManager = m_ThiefPathfindingAgent.GetComponentInParent<PathfindingManager>();
        levelIndex = LevelIndex.Jewlery;
    }

    private void OnStartMarketButton()
    {
        m_WorldManager.startMarketButton.gameObject.SetActive(false);
        m_ThiefPathfindingAgent.transform.parent = m_WorldManager.levelMarket.transform;
        m_ThiefPathfindingAgent.transform.localPosition = m_WorldManager.marketStartPosition.localPosition;
        m_ThiefPathfindingAgent.pathfindingManager = m_ThiefPathfindingAgent.GetComponentInParent<PathfindingManager>();
        levelIndex = LevelIndex.Market;
    }

    private void OnStartMuseumButton()
    {
        m_WorldManager.startMuseumButton.gameObject.SetActive(false);
        m_ThiefPathfindingAgent.transform.parent = m_WorldManager.levelMuseum.transform;
        m_ThiefPathfindingAgent.transform.localPosition = m_WorldManager.museumStartPosition.localPosition;
        m_ThiefPathfindingAgent.pathfindingManager = m_ThiefPathfindingAgent.GetComponentInParent<PathfindingManager>();
        levelIndex = LevelIndex.Museum;
    }

    private void OnStartSlumButton()
    {
        m_WorldManager.startSlumButton.gameObject.SetActive(false);
        m_ThiefPathfindingAgent.transform.parent = m_WorldManager.levelSlum.transform;
        m_ThiefPathfindingAgent.transform.localPosition = m_WorldManager.slumStartPosition.localPosition;
        m_ThiefPathfindingAgent.pathfindingManager = m_ThiefPathfindingAgent.GetComponentInParent<PathfindingManager>();
        levelIndex = LevelIndex.Slum;
    }

    private void ReadStream(PhotonStream stream)
    {
        LevelIndex state = (LevelIndex)stream.ReceiveNext();
        if (state != levelIndex)
        {
            if (thief)
            {
                if (state == LevelIndex.None)
                {
                    thief.SetActive(false);

                    // Set it in root node.
                    thief.transform.parent = null;
                }
                else
                {
                    GameObject obj = m_WorldManager.GetLevel(state);

                    if (obj)
                    {
                        thief.SetActive(true);
                        thief.transform.parent = obj.transform;
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
    }

    private void Update()
    {
        if (!thief)
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

        if (levelIndex != LevelIndex.None)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
                {
                    m_ThiefPathfindingAgent.SetDestination(hit.point, true);
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

        m_IsSetup = true;
    }

    private void SetupThief()
    {
        foreach (Thief t in FindObjectsOfType<Thief>())
        {
            PhotonView photonView = t.GetComponent<PhotonView>();
            if (photonView && m_PhotonView.OwnerActorNr == photonView.OwnerActorNr)
            {
                thief = photonView.gameObject;
                m_ThiefPathfindingAgent = thief.GetComponent<PathfindingAgent>();
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
