﻿using Photon.Pun;
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
        if (!m_IsSetup && m_PhotonView.IsMine)
        {
            m_WorldManager.startSlumButton.onClick.AddListener(OnStartSlumButton);
            m_IsSetup = true;
        }

        if (!thief)
        {
            foreach (PathfindingAgent t in FindObjectsOfType<PathfindingAgent>())
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

            return;
        }

        if (m_PhotonView.IsMine)
        {
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
    }

    private void WriteStream(PhotonStream stream)
    {
        stream.SendNext(levelIndex);
    }
}
