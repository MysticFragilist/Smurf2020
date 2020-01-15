using System.Collections;
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
    public Transform grabPos;

    GameObject box;

    private TiGuyMovement movementScript;


    // Start is called before the first frame update
    void Start()
    {
        movementScript = GetComponent<TiGuyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle Switch
        if (Input.GetKeyUp(KeyCode.E)) {
            toggle = !toggle;

            if(toggle) {
                movementScript.animator.SetBool("IsPushing", true);
            } else {
                movementScript.animator.SetBool("IsPushing", false);
            }
        }
        
        // Toggle Actions
        if (toggle) {
            if (!isPushing && !isPulling) {
                Physics2D.queriesStartInColliders = false;
                RaycastHit2D hit = Physics2D.Raycast(grabPos.position, Vector2.right * transform.localScale.x, distance, boxMask);

                if (hit.collider != null && hit.collider.gameObject.tag == "Pushable") {
                    box = hit.collider.gameObject;

                    if (box != null) {
                        if (!movementScript.isMovingObject) {
                            movementScript.isMovingObject = true;
                            isPushing = true;
                        }

                        box.GetComponent<FixedJoint2D>().enabled = true;
                        box.GetComponent<BoxPull>().beingPushed = true;
                        box.GetComponent<FixedJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
                    }
                }
            } else {
                // Si dir positive, pushing
                // Sinon pulling
                if (box != null) {
                    bool isToTheRight = Vector2.Distance(this.transform.position, box.transform.position) < 0;
                    
                    if (movementScript.controller.direction == TextureDirection.RIGHT) {
                        if(isToTheRight) {
                            isPushing = true;
                            isPulling = false;
                            movementScript.animator.SetBool("IsPushing", true);
                            movementScript.animator.SetBool("IsPulling", false);
                        } else {
                            isPushing = false;
                            isPulling = true;
                            movementScript.animator.SetBool("IsPushing", false);
                            movementScript.animator.SetBool("IsPulling", true);
                        }
                    } else if (movementScript.controller.direction == TextureDirection.LEFT) {
                        if (isToTheRight) {
                            isPushing = false;
                            isPulling = true;
                            movementScript.animator.SetBool("IsPushing", false);
                            movementScript.animator.SetBool("IsPulling", true);
                        } else {
                            isPushing = true;
                            isPulling = false;
                            movementScript.animator.SetBool("IsPushing", true);
                            movementScript.animator.SetBool("IsPulling", false);
                        }
                    }
                }
            }
        } else {
            if (box != null) {
                if (movementScript.isMovingObject) {
                    movementScript.isMovingObject = false;
                    isPulling = false;
                    isPushing = false;
                    movementScript.animator.SetBool("IsPushing", false);
                    movementScript.animator.SetBool("IsPulling", false);
                }

                box.GetComponent<FixedJoint2D>().enabled = false;
                box.GetComponent<BoxPull> ().beingPushed = false;
                box = null;
            }
        }
    }

    // Just to see where the raycast will hit
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(grabPos.position, (Vector2)grabPos.position + Vector2.right * transform.localScale.x * distance);
    }
}
