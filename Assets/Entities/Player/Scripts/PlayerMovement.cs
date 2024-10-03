using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Derived from a tutorial by Dave / GameDevelopment
 * https://www.youtube.com/watch?v=f473C43s8nE&list=PLh9SS5jRVLAleXEcDTWxBF39UjyrFc6Nb&index=7
 */

public class PlayerMovement : MonoBehaviour
{
	[Header("Movement")]
	public float moveSpeed;

	public float groundDrag;

	public float jumpForce;
	public float jumpCooldown;
	public float airMultiplier;
	bool readyToJump;

	[Header("Keybinds")]
	public KeyCode jumpKey = KeyCode.Space;

	[Header("Ground Check")]
	public float playerHeight;
	public LayerMask groundLayer;
	bool grounded;

	public Transform orientation;

	float horizontalInput;
	float verticalInput;

	Vector3 moveDirection;

	Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;
		
		readyToJump = true;
	}
	
	private void Update()
	{
		// Check to see if the player is touching the ground.
		grounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);

		MyInput();
		SpeedControl();

		//Handling the drag.
		if (grounded) rb.drag = groundDrag;
		else rb.drag = 0;
	}

	private void FixedUpdate()
	{
		MovePlayer();
	}

	private void MyInput()
	{
		horizontalInput = Input.GetAxisRaw("Horizontal");
		verticalInput = Input.GetAxisRaw("Vertical");

		// When to jump.
		if (Input.GetKey(jumpKey) && readyToJump && grounded)
		{
			readyToJump = false;

			Jump();

			Invoke(nameof(ResetJump), jumpCooldown);
		}
		else
		{
			if (!Input.GetKey(jumpKey)) Debug.Log("Jump key is not being pressed.");
			if (!readyToJump) Debug.Log("Jump boolean is false.");
			if (!grounded) Debug.Log("Player is not on ground.");
		}
	}

	private void MovePlayer()
	{
		// Calculate movement direction.
		moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

		//On Ground
		if (grounded) rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

		//in air
		else if (!grounded) rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
	}

	private void SpeedControl()
	{
		Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

		// Limits the velocity if needed.
		if (flatVelocity.magnitude > moveSpeed)
		{
			Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
			rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
		}
	}

	private void Jump()
	{
		// Reset vertical velocity.
		rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
		
		// Send player upwards.
		rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
	}

	private void ResetJump()
	{
		readyToJump = true;
	}
}
