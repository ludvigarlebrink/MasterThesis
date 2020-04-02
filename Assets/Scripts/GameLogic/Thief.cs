using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : MonoBehaviour, IPunObservable
{
    public int currentLoot = 0;
    public float currentNoise = 0;

    private PlayerHUD m_Interface;

    private void Start()
    {
        m_Interface = GetComponentInChildren<PlayerHUD>();
        if (m_Interface)
        {
            PhotonView view = GetComponent<PhotonView>();
            if (view && view.Owner != null)
            {
                m_Interface.SetName(view.Owner.NickName);
            }
            else
            {
                m_Interface.SetName("Nobody");
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

    public void ResetThief()
    {
        currentLoot = 0;
        currentNoise = 0;

        UpdateInterface();
    }

    private void ReadStream(PhotonStream stream)
    {
        currentLoot = (int)stream.ReceiveNext();
        currentNoise = (float)stream.ReceiveNext();

        UpdateInterface();
    }

    private void WriteStream(PhotonStream stream)
    {
        stream.SendNext(currentLoot);
        stream.SendNext(currentNoise);
    }

    private void Update()
    {
        UpdateInterface();
    }

    private void UpdateInterface()
    {
        if (m_Interface)
        {
            m_Interface.SetNoise(currentNoise);
            m_Interface.SetLoot(currentLoot);
        }
    }
}
