using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Derived from a tutorial by Dave / GameDevelopment
 * https://www.youtube.com/watch?v=f473C43s8nE&list=PLh9SS5jRVLAleXEcDTWxBF39UjyrFc6Nb&index=7
 */

public class MoveCam : MonoBehaviour
{
	public Transform cameraPosition;

	private void Update()
	{
		transform.position = cameraPosition.position;
	}
}
