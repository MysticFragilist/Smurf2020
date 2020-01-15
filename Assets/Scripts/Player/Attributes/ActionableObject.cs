﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum ActivableCandidate {
    TIGUY,
    GROSJEAN,
    ANY
}
public class ActionableObject : NetworkBehaviour
{
    public Sprite spriteLeft, spriteRight;
    public int forceToMakeItActive = 1;
    public ActivableCandidate CandidateToUse = ActivableCandidate.ANY;
    public List<InteracteableObject> listInteractableObjects;
    public bool isActivated {get; private set; } = false;
    public bool hasInteractableButton = true;

    public GameObject prefabButtonActive;

    private GameObject ButtonDisplayFabricated = null;
    void Update() 
    {
        if (hasInteractableButton && ButtonDisplayFabricated != null && Input.GetButtonDown("Crouch")) 
        {
            Activate(1, ActivableCandidate.GROSJEAN);
        }
    }

    public void Activate(int forceUse, ActivableCandidate candidate) 
    {
        if (forceUse >= forceToMakeItActive && (candidate == CandidateToUse || CandidateToUse == ActivableCandidate.ANY)) 
        {
            isActivated = true;
            foreach (InteracteableObject inter in listInteractableObjects)
            {
                inter.CheckIfActive();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!hasInteractableButton)
            return;

        if (other is CircleCollider2D)
            return;

        string Tag = "";
        switch(CandidateToUse) {
            case ActivableCandidate.GROSJEAN:
                Tag = "GrosJean";
                break;
        }

        if (other.tag != Tag)
            return;

        Vector3 pos = this.transform.localPosition;
        pos.y += 0.25f;
        ButtonDisplayFabricated = Instantiate(prefabButtonActive, pos, Quaternion.identity);
    }

    private void OnTriggerExit2D(Collider2D other) {
        Destroy(ButtonDisplayFabricated);
    }
}