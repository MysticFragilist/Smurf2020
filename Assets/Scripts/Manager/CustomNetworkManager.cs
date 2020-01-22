using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class CustomNetworkManager : NetworkManager
{
    Dictionary<short, GameObject> playersConnected = new Dictionary<short, GameObject>();

    public string host = "192.168.1.101";
    public List<GameObject> playerPrefabs;

    public float SpawnRadius = 0.5f;

    public void StartHosting() {
        playersConnected = new Dictionary<short, GameObject>();
        SetPort();
        
        NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnect);
        NetworkServer.RegisterHandler(MsgType.Ready, OnClientReady);
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
        SceneManager.LoadScene("MenuParticles");
    }

    void OnServerConnect(NetworkMessage msg) {
        Debug.Log("New client connected: " + msg.conn);
    }
    public override void OnStopServer()
    {
        playersConnected.Clear();
        base.OnStopServer();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        SceneManager.LoadScene("MenuParticles");
        base.OnClientDisconnect(conn);
    }
    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController playerController) {
        playersConnected.Remove(playerController.playerControllerId);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        
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
        Debug.Log("Players Connected: " + playersConnected.Count);
    }
    
    void OnClientReady(NetworkMessage msg) {
        Debug.Log("Client is ready to start: " + msg.conn);
        NetworkServer.SetClientReady(msg.conn);
        SpawnObjectWithTags("Lever");
        SpawnObjectWithTags("Elevator");
        SpawnObjectWithTags("Plate");
    }

    void SpawnObjectWithTags(string tag)
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in list) {
            NetworkServer.Spawn(obj);
        }
    }

    public override void OnClientSceneChanged(NetworkConnection conn) {
        base.OnClientSceneChanged(conn);
        Debug.Log("Server triggered scene change and we've done the same, do any extra work here for the client...");
    }
}
