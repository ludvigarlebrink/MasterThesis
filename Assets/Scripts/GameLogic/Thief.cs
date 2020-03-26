using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : MonoBehaviour, IPunObservable
{
    public int currentLoot = 0;
    public float currentNoise = 0;

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
    }

    private void ReadStream(PhotonStream stream)
    {
        currentLoot = (int)stream.ReceiveNext();
        currentNoise = (float)stream.ReceiveNext();
    }

    private void WriteStream(PhotonStream stream)
    {
        stream.SendNext(currentLoot);
        stream.SendNext(currentNoise);
    }
}
