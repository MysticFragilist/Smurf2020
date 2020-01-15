using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    List<GameObject> playersConnected = new List<GameObject>();
    
    public GameObject playerTiGuy;
    public GameObject playerGrosJean;

    public Transform spawnPoint;
    public float SpawnRadius = 0.5f;

    public void StartHosting()
    {
        SetPort();
        NetworkManager.singleton.StartHost();

    }

    public void JoinGame()
    {
        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }
   
    void SetPort()
    {
        NetworkManager.singleton.networkPort=7777;
    } 

    void SetIPAddress()
    {
        NetworkManager.singleton.networkAddress = "localhost";
    } 

    void OnLevelWasLoaded(int level)
    {
        if(level == 0)
        {
         
        }
        else
        {

        }
    }

    public void DisconnectFromHost()
    {
        NetworkManager.singleton.StopHost();
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {

        Debug.Log("player connected");
        GameObject player = null;
        Vector3 spawnPosition = spawnPoint.position;
        if (playersConnected.Count == 0) {
            spawnPosition.x += SpawnRadius;
            player = Instantiate(playerTiGuy, spawnPosition, Quaternion.identity);

        } else if (playersConnected.Count == 1) {
            spawnPosition.x -= SpawnRadius;
            player = Instantiate(playerGrosJean, spawnPosition, Quaternion.identity);
        }
        if (player != null) {
            Debug.Log("List " + playersConnected.Count);
            Debug.Log("ID " + playerControllerId);
            playersConnected.Add(player);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
    }
}
