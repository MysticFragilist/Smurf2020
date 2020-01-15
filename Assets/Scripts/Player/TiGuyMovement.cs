using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TiGuyMovement : NetworkBehaviour
{
    public TiGuyCharacterController2D controller;
    public Animator animator;
    public float runSpeed = 40f;
    public bool isMovingObject = false;

    [SyncVar]
    float horizontalMove = 0f;
    public int ID { get; private set; } = 0;

    private static int conpteID = 0;

    public override void OnStartLocalPlayer()
     {
         Camera.main.GetComponent<CameraNetworkInit>().setTarget(this.gameObject.transform);
     }


    // Start is called before the first frame update
    void Start()
    {
        conpteID ++;
        if (this.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            this.ID = conpteID;
            Debug.Log("This ID " + this.ID + " is created");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.GetComponent<NetworkIdentity>().isLocalPlayer)
            return;
        
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
    }

    public void OnLanding()
    {
      // Actually does nothing  
    }

    private void FixedUpdate() {
        if (!this.GetComponent<NetworkIdentity>().isLocalPlayer)
            return;
        
        controller.Move(horizontalMove * Time.fixedDeltaTime, isMovingObject);
    }
}
