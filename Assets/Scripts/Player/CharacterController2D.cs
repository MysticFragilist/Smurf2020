using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public enum TextureDirection {
	LEFT = -1,
	RIGHT = 1
}

public class CharacterController2D : NetworkBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings

    public float raycastLength = 0.32f;
	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;

    [SyncVar]
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]
	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{

		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

        RaycastHit2D hit = Physics2D.Raycast(m_GroundCheck.position, Vector2.down, raycastLength, m_WhatIsGround);
       
        if (hit.collider != null && hit.collider.gameObject.layer == 9) {
            m_Grounded = true;
            if (!wasGrounded)
                OnLandEvent.Invoke();
        }
	}


	public void Move(float move, bool crouch, bool jump)
	{
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
                // ... flip the player.
                CmdUpdateServerSyncedVariable(!m_FacingRight);
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
                // ... flip the player.
                CmdUpdateServerSyncedVariable(!m_FacingRight);
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}

    [Command]
    void CmdUpdateServerSyncedVariable(bool newValue) {

        m_FacingRight = newValue;

        //It's important to pass the correct updated value as a parameter in your Rpc function!
        RpcOnFaceSideChange(newValue); //or ...(serverSyncedVariable);

    }

    [ClientRpc]
    public void RpcOnFaceSideChange(bool faceRight) 
	{
        m_FacingRight = faceRight;

		Vector3 theScale = transform.localScale;
		float theScaleAbsX = Mathf.Abs(transform.localScale.x);

		if(faceRight)
			theScaleAbsX *= 1;
		else 
			theScaleAbsX *= -1;

		theScale.x = theScaleAbsX;
		transform.localScale = theScale;
	}

    // Just to see where the raycast will hit
    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(m_GroundCheck.position, (Vector2)transform.position + Vector2.down * raycastLength);
    }
}