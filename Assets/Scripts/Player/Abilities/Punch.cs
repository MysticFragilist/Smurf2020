using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    public float distance = 0.15f;
    public LayerMask boxMask;
    public Transform grabPos;

    private bool isPunching = false;
    private bool hasHit = false;

    private float punchingTimer = 0f;
    private float punchCountdown = 0.8f;

    private Animator anim;

    void Awake() {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F) && !isPunching) {
            isPunching = true;

            punchingTimer = punchCountdown;
        }

        if (isPunching) {
            if (punchingTimer > 0) {
                punchingTimer -= Time.deltaTime;
            } else {
                isPunching = false;
                hasHit = false;
            }

            if (punchingTimer >= 0.4f && !hasHit) {
                hasHit = true;
                RaycastHit2D hit = Physics2D.Raycast(grabPos.position, Vector2.right * transform.localScale.x, distance, boxMask);
                
                if (hit.collider != null && hit.collider.gameObject.tag == "Destroyable") {
                    hit.collider.gameObject.transform.position = new Vector2(999999, 99999);
                }
            }
        }

        anim.SetBool("IsPunching", isPunching);
    }

    // Just to see where the raycast will hit
    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(grabPos.position, (Vector2)grabPos.position + Vector2.right * transform.localScale.x * distance);
    }
}
