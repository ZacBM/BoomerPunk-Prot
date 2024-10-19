/*
 * Derived from a tutorial by Dave / GameDevelopment
 * https://www.youtube.com/watch?v=f473C43s8nE&list=PLh9SS5jRVLAleXEcDTWxBF39UjyrFc6Nb&index=7
 */

using UnityEngine;

[RequireComponent(typeof(PlayerBase))]

public class PlayerMovement : MonoBehaviour
{
	private PlayerBase playerBase;
	
	[Header("Movement")]
	public float moveSpeed;

	public float groundDrag;

	public float jumpForce;
	public float airMultiplier;

	[Header("Ground Check")]
	public float playerHeight;
	public LayerMask groundLayer;

	public Transform cameraTransform;

	Vector3 moveDirection;

	Rigidbody rigidbody;
	
	public LayerMask exitLayer;

	private PlayerSliding playerSliding;

	[SerializeField] private float gravityScale;

	private void Awake()
	{
		playerBase = GetComponent<PlayerBase>();
		playerSliding = GetComponent<PlayerSliding>();
		rigidbody = GetComponent<Rigidbody>();
	}
	
	private void Start()
	{
		rigidbody.freezeRotation = true;
    }

    private void Update()
    {
	    SpeedControl();

	    if (IsGrounded() && playerBase.jump.triggered)
	    {
		    Jump();
	    }
    }

    private void FixedUpdate()
	{
		MovePlayer();
	}

	public bool IsGrounded()
	{
		return Physics.Raycast(transform.position + Vector3.up, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
	}

	private void ApplyGravity()
	{
		Vector3 gravity = Physics.gravity * gravityScale;
		rigidbody.AddForce(gravity, ForceMode.Acceleration);
	}

	private void MovePlayer()
	{
		float inputX = playerBase.movementDirection.x;
		float inputZ = playerBase.movementDirection.y;
		moveDirection = (cameraTransform.right * inputX) + (cameraTransform.forward * inputZ);

        float appliedMoveSpeed = moveSpeed;

        if (playerSliding != null && playerSliding.isSliding)
        {
	        appliedMoveSpeed *= playerSliding.GetCurrentSpeedMultiplier();
        }

        //On Ground
        if (IsGrounded())
        {
	        rigidbody.AddForce(moveDirection.normalized * appliedMoveSpeed * 10f, ForceMode.Force);
        }
		else
		{
			rigidbody.AddForce(moveDirection.normalized * appliedMoveSpeed * 10f * airMultiplier, ForceMode.Force);
		}
        
		ApplyGravity();
    }

	private void SpeedControl()
	{
		Vector3 flatVelocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);

		// Limits the velocity if needed.
		if (flatVelocity.magnitude > moveSpeed)
		{
			Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
			rigidbody.velocity = new Vector3(limitedVelocity.x, rigidbody.velocity.y, limitedVelocity.z);
		}
	}

	private void Jump()
	{
		Vector3 zeroVerticalVelocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
		rigidbody.velocity = zeroVerticalVelocity;
		
		rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
	}
}
