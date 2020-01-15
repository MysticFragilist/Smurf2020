using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleableBlockScript : MonoBehaviour
{
    public bool defaultStateActive = false;

    public void OnActiveToggle() {
        if(defaultStateActive) 
        {
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            this.GetComponent<BoxCollider2D>().enabled = true;
        }
        defaultStateActive = !defaultStateActive;
    }
}
