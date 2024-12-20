/*
 * Derived from a tutorial by Dave / GameDevelopment
 * https://www.youtube.com/watch?v=f473C43s8nE&list=PLh9SS5jRVLAleXEcDTWxBF39UjyrFc6Nb&index=7
 */

using UnityEngine;

public class MoveCam : MonoBehaviour
{
	/// <remarks>
	/// This script should get deleted once the Player Camera Holder prefab is removed.
	///  
	/// - Joshua  
	/// </remarks>
	
	public Transform cameraPosition;

	private void Update()
	{
		if (cameraPosition == null)
		{
			cameraPosition = GameObject.FindGameObjectWithTag("Camera Position").transform;
		}

		if (cameraPosition != null)
		{
			transform.position = cameraPosition.position;
		}
	}
}
