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
    public bool isActivated {get; private set; } = false;
    public bool isLeverHandle = true;
    public GameObject prefabButtonActive;
    private GameObject ButtonDisplayFabricated = null;
    void Update() 
    {
        if (isLeverHandle && ButtonDisplayFabricated != null && Input.GetButtonDown("Fire1")) 
        {
            if (GameObject.FindGameObjectWithTag("GrosJean").GetComponent<CharacterController2D>().isLocalPlayer)
            {
                Debug.Log("Gros tas");
            }
                Activate(1, ActivableCandidate.GROSJEAN);
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
    public void CmdServerSpriteShare(Sprite newSprite) {
        Debug.Log("Send");
        RpcClientSendServerSprite(newSprite);
    }

    [ClientRpc]
    public void RpcClientSendServerSprite(Sprite newSprite) {
        Debug.Log("Receive");
        this.GetComponent<SpriteRenderer>().sprite = newSprite;
    }
}