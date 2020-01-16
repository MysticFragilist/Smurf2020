using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
    Dictionary<short, GameObject> playersConnected = new Dictionary<short, GameObject>();

    public string host = "localhost";
    public List<GameObject> playerPrefabs;

    public float SpawnRadius = 0.5f;

    public void StartHosting() {
        playersConnected = new Dictionary<short, GameObject>();
        SetPort();
        NetworkManager.singleton.StartHost();
    }

    public void JoinGame() {
        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }
   
    void SetPort() {
        NetworkManager.singleton.networkPort = 7777;
    }

    void SetIPAddress() {
        NetworkManager.singleton.networkAddress = host;
    }

    public void DisconnectFromHost() {
        NetworkManager.singleton.StopHost();
        // TODO: Revenir a la scene du menu
    }
        
    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController playerController) {
        playersConnected.Remove(playerController.playerControllerId);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        Debug.Log("Players Connected: " + playersConnected.Count);
        GameObject player = null;
        Vector3 spawnPosition = GetStartPosition().position;

        // The host is connecting
        if (playersConnected.Count == 0) {
            spawnPosition.x += SpawnRadius;
            player = Instantiate(playerPrefabs[0], spawnPosition, Quaternion.identity);

        // A second client is connecting after the host
        } else if (playersConnected.Count == 1) {
            spawnPosition.x -= SpawnRadius;
            player = Instantiate(playerPrefabs[1], spawnPosition, Quaternion.identity);
        }

        // Publish the connection over all clients
        if (player != null) {
            if (!playersConnected.ContainsKey(playerControllerId)) {
                playersConnected.Add(playerControllerId, player);
            }
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
    }
}
