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
    [SyncVar]
    public int forceToMakeItActive = 1;
    [SyncVar]
    public ActivableCandidate CandidateToUse = ActivableCandidate.ANY;
    public List<InteracteableObject> listInteractableObjects;
    
    [SyncVar]
    public bool isActivated = false;
    [SyncVar]
    public bool isLeverHandle = true;
    public GameObject prefabButtonActive;
    private GameObject ButtonDisplayFabricated = null;
    void Update() 
    {
        if (isLeverHandle && ButtonDisplayFabricated != null && Input.GetButtonDown("Fire1")) 
        {
            if (GameObject.FindGameObjectWithTag("GrosJean").GetComponent<CharacterController2D>().isLocalPlayer)
            {
                
                if (!isActivated) CmdActivate(1, ActivableCandidate.GROSJEAN);
                else CmdDeactivate();
            }
        }
    }

    [Command]
    public void CmdActivate(int forceUse, ActivableCandidate candidate)
    {
        if (isActivated) return;

        if (forceUse >= forceToMakeItActive && (candidate == CandidateToUse || CandidateToUse == ActivableCandidate.ANY)) 
        {
            CmdChangeActivate(true);
            CmdServerSpriteShare(isActivated);
            
            foreach (InteracteableObject inter in listInteractableObjects)
            {
                inter.CheckIfActive();
            }
        }
    }

    [Command]
    public void CmdDeactivate()
    {
        if (!isActivated) return;

        CmdChangeActivate(false);
        CmdServerSpriteShare(isActivated);
            
        foreach (InteracteableObject inter in listInteractableObjects)
        {
            inter.CheckIfActive();
        }
    }

    [Command]
    public void CmdChangeActivate(bool newActivated) {
        isActivated = newActivated;
        RpcChangeActivate(newActivated);
    }

    [ClientRpc]
    private void RpcChangeActivate(bool newActivated) {
        Debug.Log("Is Activated : " + newActivated);
        this.isActivated = newActivated;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!isLeverHandle) {
            if (other.tag == "TiGuy") {
                if (other.gameObject.GetComponent<TiGuyCharacterController2D>().isLocalPlayer) {
                    CmdActivate(20, ActivableCandidate.TIGUY);
                }
            }
            else if (other.tag == "Pushable") {
                CmdActivate(5, ActivableCandidate.TIGUY);
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
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(!isLeverHandle) {
            if (other.tag == "TiGuy") {
                if (other.gameObject.GetComponent<TiGuyCharacterController2D>().isLocalPlayer) {
                    CmdDeactivate();
                }
            }
            else if (other.tag == "Pushable") {
                CmdDeactivate();
            }
        }
        else {
            Destroy(ButtonDisplayFabricated);
        }
    }

    [Command]
    public void CmdServerSpriteShare(bool newActivated) {
        isActivated = newActivated;
        RpcClientSendServerSprite(isActivated);
    }

    [ClientRpc]
    public void RpcClientSendServerSprite(bool newActivated) {
        isActivated = newActivated;

        if(isActivated) this.GetComponent<SpriteRenderer>().sprite = spriteActive;
        else this.GetComponent<SpriteRenderer>().sprite = spriteInactive;
    }
}