/*
 * Derived from a tutorial by Dave / GameDevelopment.
 * https://www.youtube.com/watch?v=f473C43s8nE&list=PLh9SS5jRVLAleXEcDTWxBF39UjyrFc6Nb&index=7
 */

using UnityEngine;

public class PlayerCam : MonoBehaviour
{
	private PlayerBase playerBase;
	
	// Sensitivity values for both the X & Y axes.
	[SerializeField] private float sensitivityX = 450.0f;
	[SerializeField] private float sensitivityY = 450.0f;

	public Transform playerOrientation;

	private float cameraXRotation;
	private float cameraYRotation;

	private void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked; // Locks mouse to the middle of the screen.
		Cursor.visible = false;
	}

	private void Start()
	{
		playerBase = FindObjectOfType<PlayerBase>();
	}

	private void Update()
	{
		// Retrieve mouse input here.
		float mouseX = (Input.GetAxisRaw("Mouse X") + playerBase.lookDirection.x) * sensitivityX * Time.deltaTime;
		float mouseY = (Input.GetAxisRaw("Mouse Y") + playerBase.lookDirection.y) * sensitivityY * Time.deltaTime;

		cameraYRotation += mouseX;
		cameraXRotation -= mouseY;
		
		cameraXRotation = Mathf.Clamp(cameraXRotation, -90f, 90f); // Prevents looking up or down more than is natural.

		//Rotatation of cam and orientation.
		transform.rotation = Quaternion.Euler(cameraXRotation, cameraYRotation, 0f);
		if (playerOrientation == null)
		{
			playerOrientation = GameObject.FindGameObjectWithTag("Player Orientation").transform;
		}

		if (playerOrientation != null)
		{
			playerOrientation.rotation = Quaternion.Euler(0f, cameraYRotation, 0f);
		}
	}
}
