using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameMaster gm;

    public Sprite sprite1; // Drag your first sprite here
    public Sprite sprite2; // Drag your second sprite here

    private SpriteRenderer spriteRenderer; 

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("TiGuy") || other.CompareTag("GrosJean")){
            gm.lastCheckPointPos = transform.position;
            spriteRenderer.sprite = sprite2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
