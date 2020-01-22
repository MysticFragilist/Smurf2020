using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkTestScript : NetworkBehaviour
{
    bool isActive = false;
    public Sprite spriteInactive, spriteActive;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        isActive = true;
        CmdServerSpriteShare(isActive);
    }

    private void OnTriggerExit2D(Collider2D other) {
        isActive = false;
        CmdServerSpriteShare(isActive);
    }

    [Command]
    public void CmdServerSpriteShare(bool isActivated) {
        RpcClientSendServerSprite(isActivated);
    }

    [ClientRpc]
    public void RpcClientSendServerSprite(bool isActivated) {
        if(isActivated) this.GetComponent<SpriteRenderer>().sprite = spriteActive;
        else this.GetComponent<SpriteRenderer>().sprite = spriteInactive;
    }

}
