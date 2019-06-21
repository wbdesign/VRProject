using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolster : MonoBehaviour
{
	private Transform[] waypoints = new Transform[2];

	public Transform upTransform;
	public Transform holsteredTransform;
	public float time = 8;
	private float elapsedTime = 0;

	bool pathing = false;

	private void Start()
	{
		waypoints[1] = holsteredTransform;
	}


	public void GoUp()
	{
		elapsedTime = 0;
		pathing = true;
		waypoints[0] = holsteredTransform;
		waypoints[1] = upTransform;
	}

	public void Holster()
	{
		elapsedTime = 0;
		pathing = true;
		waypoints[0] = upTransform;
		waypoints[1] = holsteredTransform;
	}

	void Update()
	{
		// NOT pathing
		if (!pathing)
		{
			// set transform to end transform
			transform.position = waypoints[1].position;
			transform.rotation = waypoints[1].rotation;

			return;
		}
		
		// Pathing so lerp to end position
		float lerpTime = elapsedTime / time;
		transform.position = Vector3.Lerp(waypoints[0].position, waypoints[1].position, lerpTime);
		
		// Stop pathing when end is reached
		if (lerpTime >= 1.0f)
		{
			elapsedTime = 0;
			pathing = false;
		}
		
		return;
	}
}
