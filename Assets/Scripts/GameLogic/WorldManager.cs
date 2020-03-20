using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject levelBank;
    [SerializeField] private GameObject levelBlackMarket;
    [SerializeField] private GameObject levelHarbour;
    [SerializeField] private GameObject levelJewlery;
    [SerializeField] private GameObject levelMarket;
    [SerializeField] private GameObject levelMuseum;
    [SerializeField] private GameObject levelSlum;

    public GameObject GetLevel(LevelIndex levelIndex)
    {
        switch (levelIndex)
        {
            case LevelIndex.None:
                return null;
            case LevelIndex.Bank:
                return levelBank;
            case LevelIndex.BlackMarket:
                return levelBlackMarket;
            case LevelIndex.Harbour:
                return levelHarbour;
            case LevelIndex.Jewlery:
                return levelJewlery;
            case LevelIndex.Market:
                return levelMarket;
            case LevelIndex.Museum:
                return levelMuseum;
            case LevelIndex.Slum:
                return levelSlum;
            default:
                break;
        }

        return null;
    }

    private void Awake()
    {
        // Only on the master client.
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                GameObject clientObject = PhotonNetwork.Instantiate("Prefabs/Client", Vector3.zero, Quaternion.identity);

                PhotonView clientPhotonView = clientObject.GetComponent<PhotonView>();
                if (!clientPhotonView)
                {
                    Debug.LogError("Prefab does not contain PhotonView component.");
                    return;
                }

                clientPhotonView.TransferOwnership(player);

                ClientSubject client = clientObject.GetComponent<ClientSubject>();
                if (!client)
                {
                    Debug.LogError("Prefab does not contain Client component.");
                    return;
                }

                GameObject thiefObject = PhotonNetwork.Instantiate("Prefabs/Theif", Vector3.zero, Quaternion.identity);
                
                PhotonView thiefPhotonView = thiefObject.GetComponent<PhotonView>();
                if (!thiefPhotonView)
                {
                    Debug.LogError("Prefab does not contain PhotonView component.");
                    return;
                }

                thiefPhotonView.TransferOwnership(player);
            }
        }
    }

    private void Update()
    {

    }
}
