using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPos : MonoBehaviour
{

    private GameMaster gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        transform.position = gm.lastCheckPointPos;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad5)){
            RpcSendTeleport();
        }
        
    }

    [ClientRpc]
    public void RpcSendTeleport(){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            transform.position = gm.lastCheckPointPos;
    }
}
