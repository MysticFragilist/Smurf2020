﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPull : MonoBehaviour
{

    public bool beingPushed;
    float xPos;
    bool grabbed = false;
    public float height = 0.03f;
    
    // Start is called before the first frame update
    void Start()
    {
        xPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!beingPushed) {
            transform.position = new Vector3 (xPos, transform.position.y);
            grabbed = false;

        } else{
            xPos = transform.position.x;
            if(!grabbed){
                transform.position = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
                grabbed = true;
            }
        }

    }
}
