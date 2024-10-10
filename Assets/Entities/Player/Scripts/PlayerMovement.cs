/*
 * Derived from a tutorial by Dave / GameDevelopment
 * https://www.youtube.com/watch?v=f473C43s8nE&list=PLh9SS5jRVLAleXEcDTWxBF39UjyrFc6Nb&index=7
 */

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	public bool grounded;

	public Transform orientation;

	float horizontalInput;
	float verticalInput;

	Vector3 moveDirection;

	Rigidbody rigidbody;
	
	public LayerMask exitLayer;
	
	[SerializeField] private HPComponent hpComponent;

	private Sliding sliding;

	[SerializeField] private float gravityScale;

    private void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.freezeRotation = true;
		
		readyToJump = true;

        sliding = GetComponent<Sliding>();
    }
	
	private void Update()
	{
		// Check to see if the player is touching the ground.
		grounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);

		MyInput();
		SpeedControl();

		//Handling the drag.
		if (grounded) rigidbody.drag = groundDrag;
		else rigidbody.drag = 0f;

        if (hpComponent != null && hpComponent.health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
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
	}

	private void ApplyGravity()
	{
		Vector3 gravity = Physics.gravity * gravityScale;
		rigidbody.AddForce(gravity, ForceMode.Acceleration);
	}

	private void MovePlayer()
	{
		// Calculate movement direction.
		moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        float appliedMoveSpeed = moveSpeed;

        if (sliding != null && sliding.sliding) appliedMoveSpeed *= sliding.GetCurrentSpeedMultiplier();

        //On Ground
        if (grounded) rigidbody.AddForce(moveDirection.normalized * appliedMoveSpeed * 10f, ForceMode.Force);

		//in air
		else if (!grounded) rigidbody.AddForce(moveDirection.normalized * appliedMoveSpeed * 10f * airMultiplier, ForceMode.Force);
        
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
		// Reset vertical velocity.
		rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
		
		// Send player upwards.
		rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
	}

	private void ResetJump()
	{
		readyToJump = true;
	}
}
