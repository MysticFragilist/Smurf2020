using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCode : MonoBehaviour
{

    public float distance = 5f;
    public LayerMask boxMask;
    public Transform grabPos;

    private ElevatorObject elevator;

    void Start()
    {
        
    }

    void Update()
    {
        /*RaycastHit2D RayRight = Physics2D.Raycast(grabPos.position, Vector2.right * transform.localScale.x, distance, boxMask);
        RaycastHit2D RayLeft = Physics2D.Raycast(grabPos.position, Vector2.left * transform.localScale.x, distance, boxMask);
        RaycastHit2D RayUp = Physics2D.Raycast(grabPos.position, Vector2.up * transform.localScale.y, distance, boxMask);*/
        if(Input.GetKeyUp(KeyCode.Keypad1) && Input.GetKeyUp(KeyCode.Keypad2) && Input.GetKeyUp(KeyCode.Keypad3)){
            RaycastHit2D hit = Physics2D.Raycast(grabPos.position, Vector2.down * transform.localScale.y, distance, boxMask);
            if(hit.collider != null && hit.collider.gameObject.GetComponent<InteracteableObject>() != null){
                Debug.Log(hit.collider.name);
                hit.collider.gameObject.GetComponent<InteracteableObject>().OnActiveEvent.Invoke();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(grabPos.position, (Vector2)grabPos.position + Vector2.down * transform.localScale.y * distance);
    }
}
