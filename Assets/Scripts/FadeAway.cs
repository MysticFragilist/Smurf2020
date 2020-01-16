using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "TiGuy" || other.gameObject.tag == "GrosJean"){
            Debug.Log("has entered");
            gameObject.SetActive(false);
        }
    }
}
