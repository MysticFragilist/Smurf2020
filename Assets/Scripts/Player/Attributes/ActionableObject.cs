using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public enum ActivableCandidate {
    TIGUY,
    GROSJEAN,
    ANY
}
public class ActionableObject : NetworkBehaviour
{
    public Sprite spriteInactive, spriteActive;
    public int forceToMakeItActive = 1;
    public ActivableCandidate CandidateToUse = ActivableCandidate.ANY;
    public List<InteracteableObject> listInteractableObjects;

    [SyncVar]
    bool isActivated = false;

    public bool IsActivated { get { return isActivated; } }
    public bool isLeverHandle = true;
    public GameObject prefabButtonActive;
    private GameObject ButtonDisplayFabricated = null;
    private GameObject playerThatHasIntersect = null;
    public override void OnStartClient() {
        Debug.Log("Spawn in server");
    }
    void Update() 
    {
        if (isLeverHandle && Input.GetButtonDown("Crouch")) 
        {
            if (playerThatHasIntersect == FindNetworkPlayer("GrosJean"))
            {
                Activate(1, ActivableCandidate.GROSJEAN);
            }
        }
    }
    public void Activate(int forceUse, ActivableCandidate candidate)
    {
        if (forceUse >= forceToMakeItActive && (candidate == CandidateToUse || CandidateToUse == ActivableCandidate.ANY)) 
        {
            isActivated = !isActivated;
            if (isActivated) CmdServerSpriteShare(spriteActive);
            else CmdServerSpriteShare(spriteInactive);
            
            foreach (InteracteableObject inter in listInteractableObjects)
            {
                inter.CheckIfActive();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!isLeverHandle) {
            if (other.tag == "TiGuy") {
                if (other.gameObject.GetComponent<TiGuyCharacterController2D>().isLocalPlayer) {
                    Activate(20, ActivableCandidate.TIGUY);
                }
            }
            else if (other.tag == "Pushable") {
                Activate(5, ActivableCandidate.TIGUY);
            }
            return;
        }

        if (other is CircleCollider2D)
            return;

        string Tag = "";
        switch(CandidateToUse) {
            case ActivableCandidate.GROSJEAN:
                Tag = "GrosJean";
                break;
            case ActivableCandidate.TIGUY:
                Tag = "TiGuy";
                break;
        }
        if (other.tag != Tag)
            return;

        if(!other.GetComponent<CharacterController2D>().isLocalPlayer)
            return;

        Vector3 pos = this.transform.localPosition;
        pos.y += 0.25f;
        ButtonDisplayFabricated = Instantiate(prefabButtonActive, pos, Quaternion.identity);
        playerThatHasIntersect = other.gameObject;
        NetworkServer.Spawn(ButtonDisplayFabricated);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(!isLeverHandle) {
            if (other.tag == "TiGuy") {
                if (other.gameObject.GetComponent<TiGuyCharacterController2D>().isLocalPlayer) {
                    Activate(20, ActivableCandidate.TIGUY);
                }
            }
            else if (other.tag == "Pushable") {
                Activate(5, ActivableCandidate.TIGUY);
            }
        }
        else {
            Destroy(ButtonDisplayFabricated);
        }
    }

    [Command]
    public void CmdServerSpriteShare(bool newSpriteState) {
        Debug.Log("Send");
        RpcClientSendServerSprite(newSpriteState);
    }

    [ClientRpc]
    public void RpcClientSendServerSprite(bool newSpriteState) {
        Debug.Log("Receive");
        
        if(newSpriteState) this.GetComponent<SpriteRenderer>().sprite = spriteActive;
        else this.GetComponent<SpriteRenderer>().sprite = spriteInactive;
    }


    GameObject FindNetworkPlayer(string name)
    {
        NetworkManager networkManager = NetworkManager.singleton;
        List<PlayerController> pc = networkManager.client.connection.playerControllers;

        for (int i = 0; i < pc.Count; i++)
        {
            if ((pc[i].IsValid) && (name == pc[i].gameObject.tag))
                return pc[i].gameObject;
        }
        return null;
    }
}