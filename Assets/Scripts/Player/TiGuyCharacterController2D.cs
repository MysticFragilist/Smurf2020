using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class TiGuyCharacterController2D : NetworkBehaviour
{
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;

    [SyncVar]
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    public TextureDirection direction = TextureDirection.RIGHT;
	private Vector3 m_Velocity = Vector3.zero;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }
    
	private bool m_wasCrouching = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		if(!isLocalPlayer)
			return;

		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
			}
		}
	}


	public void Move(float move, bool isMovingObject)
	{
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            /*
			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight && !isMovingObject)
			{
				// ... flip the player.
				m_FacingRight = !m_FacingRight;
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight && !isMovingObject)
			{
				// ... flip the player.
				m_FacingRight = !m_FacingRight;
			}
            */

            if (move > 0) {
                if (isMovingObject) {
                    // Going right
                    direction = TextureDirection.RIGHT;
                }

                // If the input is moving the player right and the player is facing left...
                if(!m_FacingRight && !isMovingObject) {
                    // ... flip the player.
                    CmdUpdateServerSyncedVariable(!m_FacingRight);
                }
                
            } else if (move < 0) {
                if (isMovingObject) {
                    // Going left
                    direction = TextureDirection.LEFT;
                }

                // Otherwise if the input is moving the player left and the player is facing right...
                if (m_FacingRight && !isMovingObject) {
                    // ... flip the player.
                    CmdUpdateServerSyncedVariable(!m_FacingRight);
                }
            }

        }
	}

    [Command]
    void CmdUpdateServerSyncedVariable(bool newValue) {

        m_FacingRight = newValue;

        //It's important to pass the correct updated value as a parameter in your Rpc function!
        RpcOnFaceSideChange(newValue); //or ...(serverSyncedVariable);

    }

    [ClientRpc]
    public void RpcOnFaceSideChange(bool faceRight) {
        m_FacingRight = faceRight;

        Vector3 theScale = transform.localScale;
        float theScaleAbsX = Mathf.Abs(transform.localScale.x);

        if (faceRight)
            theScaleAbsX *= 1;
        else
            theScaleAbsX *= -1;

        theScale.x = theScaleAbsX;
        transform.localScale = theScale;
    }
}