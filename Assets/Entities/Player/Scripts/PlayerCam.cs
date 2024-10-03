using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Derived from a tutorial by Dave / GameDevelopment.
 * https://www.youtube.com/watch?v=f473C43s8nE&list=PLh9SS5jRVLAleXEcDTWxBF39UjyrFc6Nb&index=7
 */

public class PlayerCam : MonoBehaviour
{
	// Sensitivity values for both the X & Y axes.
	public float sensitivityX;
	public float sensitivityY;

	public Transform playerOrientation;

	float cameraXRotation;
	float cameraYRotation;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked; // Locks mouse to the middle of the screen.
		Cursor.visible = false;
	}

	private void Update()
	{
		// Retrieve mouse input here.
		float mouseX = Input.GetAxisRaw("Mouse X") * sensitivityX * Time.deltaTime;
		float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivityY * Time.deltaTime;

		cameraYRotation += mouseX;

		cameraXRotation -= mouseY;
		cameraXRotation = Mathf.Clamp(cameraXRotation, -90f, 90f); // Prevents looking up or down more than is natural.

		//Rotatation of cam and orientation.
		transform.rotation = Quaternion.Euler(cameraXRotation, cameraYRotation, 0f);
		playerOrientation.rotation = Quaternion.Euler(0f, cameraYRotation, 0f);
	}
}
