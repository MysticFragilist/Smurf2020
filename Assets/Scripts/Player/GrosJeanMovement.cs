using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GrosJeanMovement : NetworkBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public float runSpeed = 40f;

    [SyncVar]
    float horizontalMove = 0f;
    [SyncVar]
    bool jump = false;
    public int ID { get; private set; } = 0; 

    private static int compteID = 0;


    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<CameraNetworkInit>().setTarget(this.gameObject.transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        compteID ++;
        if (isLocalPlayer) 
        {
            this.ID = compteID;
            Debug.Log("This ID " + this.ID + " is created");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;
        
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        if (Input.GetButtonDown("Jump")) {
            jump = true;
            animator.SetBool("IsJumping", true);
            Debug.Log("IsJumping - True");
        }
    }

    public void OnLanding() 
    {
        animator.SetBool("IsJumping", false);
        Debug.Log("IsJumping - False");
    }

    private void FixedUpdate() {
        if (!isLocalPlayer)
            return;
        
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);

        jump = false;
    }
}
