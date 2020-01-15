using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour
{

    private static int playerCount = 0;
    public GameObject player2;
    public NetworkManager netMgr;

    bool isAlreadyConnected = false;

    private void Update() {

        if(netMgr.numPlayers == 1 && !isAlreadyConnected) 
        {
            netMgr.playerPrefab = player2;
            isAlreadyConnected = true;
        }
    }
}
