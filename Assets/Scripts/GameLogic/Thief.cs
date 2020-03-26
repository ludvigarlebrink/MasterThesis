using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : MonoBehaviour, IPunObservable
{
    public int currentLoot = 0;
    public float currentNoise = 0;

    private PlayerHUD m_interface;

    private void Start()
    {
        m_interface = GetComponentInChildren<PlayerHUD>();
        if (m_interface)
        {
            PhotonView view = GetComponent<PhotonView>();
            if (view && view.Owner != null)
            {
                m_interface.SetName(view.Owner.NickName);
            }
            else
            {
                m_interface.SetName("Nobody");
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
        if (m_interface)
        {
            m_interface.SetNoise(currentNoise);
            m_interface.SetLoot(currentLoot);
        }
    }
}
