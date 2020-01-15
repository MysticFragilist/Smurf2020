using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TiGuyMovement : NetworkBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public float runSpeed = 40f;

    [SyncVar]
    float horizontalMove = 0f;
    [SyncVar]
    bool crouch = false;
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
        animator.SetBool("IsJumping", false);
    }

    private void FixedUpdate() {
        if (!this.GetComponent<NetworkIdentity>().isLocalPlayer)
            return;
        
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, false);
    }
}
