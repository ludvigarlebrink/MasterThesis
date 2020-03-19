using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClientState
{
    Idle,
    LevelBank,
    LevelBlackMarket,
    LevelHarbour,
    LevelMarket,
    LevelSlum,
    LevelJewlery,
    LevelMuseum
}

[RequireComponent(typeof(PhotonView))]
public class ClientSubject : MonoBehaviour, IPunObservable
{
    public GameObject m_Theif = null;
    public ClientState m_ClientState = ClientState.Idle;
 
    private PhotonView m_PhotonView = null;

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

    private void ReadStream(PhotonStream stream)
    {
        ClientState state = (ClientState)stream.ReceiveNext();
        if (state != m_ClientState)
        {
            m_ClientState = state;
        }


    }

    private void Start()
    {
        m_PhotonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!m_PhotonView.IsMine)
        {
            return;
        }
    }

    private void WriteStream(PhotonStream stream)
    {
        stream.SendNext(m_ClientState);
    }
}
