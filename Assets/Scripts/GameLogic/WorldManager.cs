using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        // Only on the master client.
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                GameObject clientObject = PhotonNetwork.Instantiate("Prefabs/Client", Vector3.zero, Quaternion.identity);

                PhotonView photonView = clientObject.GetComponent<PhotonView>();
                if (!photonView)
                {
                    Debug.LogError("Prefab does not contain PhotonView component.");
                    return;
                }

                photonView.TransferOwnership(player);

                ClientSubject client = clientObject.GetComponent<ClientSubject>();
                if (!client)
                {
                    Debug.LogError("Prefab does not contain Client component.");
                    return;
                }
            }
        }
    }

    private void Update()
    {
    }
}
