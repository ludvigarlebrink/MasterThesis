using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldManager : MonoBehaviourPunCallbacks
{
    public GameObject levelBank;
    public GameObject levelBlackMarket;
    public GameObject levelHarbour;
    public GameObject levelJewlery;
    public GameObject levelMarket;
    public GameObject levelMuseum;
    public GameObject levelSlum;

    public Transform slumStartPosition;

    public Button startBankButton;
    public Button startBlackMarketButton;
    public Button startHarbourButton;
    public Button startJewleryButton;
    public Button startMarketButton;
    public Button startMuseumButton;
    public Button startSlumButton;

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

                GameObject thiefObject = PhotonNetwork.Instantiate("Prefabs/Thief", Vector3.zero, Quaternion.identity);
                
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
