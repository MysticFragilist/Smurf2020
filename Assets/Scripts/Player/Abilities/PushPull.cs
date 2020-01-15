﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPull : MonoBehaviour
{

    public float distance = 1f;
    public LayerMask boxMask;
    private bool toggle = false;
    private int compteur = 0;

    private bool isPulling = false;
    private bool isPushing = false;

    GameObject box;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.E)){
            toggle = !toggle;
        }

        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.right * transform.localScale.x, distance, boxMask);

        if(hit.collider != null && hit.collider.gameObject.tag == "Pushable" && toggle)
        {
            box = hit.collider.gameObject;

            box.GetComponent<FixedJoint2D> ().enabled = true;
            box.GetComponent<BoxPull> ().beingPushed = true;
            box.GetComponent<FixedJoint2D> ().connectedBody = this.GetComponent<Rigidbody2D> ();

        } else if (!toggle)
        {
            box.GetComponent<FixedJoint2D> ().enabled = false;
            box.GetComponent<BoxPull> ().beingPushed = false;
        }
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * distance);
    }
}