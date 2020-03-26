using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Level : MonoBehaviour, IPunObservable
{
    private PhotonView m_PhotonView = null;
    private int m_ActorNr = -1;

    private UnityAction<bool> m_Callback = null;

    public int actorNr
    {
        get
        {
            return m_ActorNr;
        }
    }

    public void TryToSetOwner(int actor, UnityAction<bool> callback)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (m_ActorNr == -1)
            {

                m_ActorNr = actor;
                callback.Invoke(true);
            }
            else
            {
                callback.Invoke(false);
            }
        }
        else
        {
            if (m_ActorNr == -1)
            {
                callback.Invoke(false);
            }
            else
            {
                m_PhotonView.RPC("RPCSetOwner", RpcTarget.MasterClient, actor);
                m_Callback = callback;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
        {
            ReadStream(stream);
        }
        else
        {
            WriteStream(stream);
        }
    }

    private void ReadStream(PhotonStream stream)
    {
        int tmpOwner = (int)stream.ReceiveNext();
        if (tmpOwner != m_ActorNr)
        {
            if (tmpOwner == -1)
            {

            }
            else
            {

            }

            m_ActorNr = tmpOwner;
        }
    }

    [PunRPC]
    private void RPCOnTryingToChangeOwner(bool changedOwner)
    {
        if (m_Callback != null)
        {
            m_Callback.Invoke(changedOwner);
            m_Callback = null;
        }
    }

    [PunRPC]
    private void RPCSetOwner(int actor)
    {
        if (m_ActorNr == -1)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.ActorNumber == actor)
                {
                    m_PhotonView.RPC("RPCOnTryingToChangeOwner", player, true);
                    m_ActorNr = actor;
                }
            }
        }
        else if (actor == -1)
        {
            m_ActorNr = -1;
        }
    }

    private void Start()
    {
        m_PhotonView = GetComponent<PhotonView>();
    }

    private void WriteStream(PhotonStream stream)
    {
        stream.SendNext(m_ActorNr);
    }
}
