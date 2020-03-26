using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour, IPunObservable
{
    private PhotonView m_PhotonView = null;
    private int m_Owner = -1;

    public int owner
    {
        get
        {
            return m_Owner;
        }
    }

    public void TryToSetOwner(int owner, UnityAction<bool> callback)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            m_Owner = owner;
            callback.Invoke(true);
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

    private void Start()
    {
        m_PhotonView = GetComponent<PhotonView>();
    }

    private void ReadStream(PhotonStream stream)
    {
        int tmpOwner = (int)stream.ReceiveNext();
        if (tmpOwner != m_Owner)
        {
            if (tmpOwner == -1)
            {

            }
            else
            {

            }

            m_Owner = tmpOwner;
        }
    }

    private void WriteStream(PhotonStream stream)
    {
        stream.SendNext(m_Owner);
    }
}
