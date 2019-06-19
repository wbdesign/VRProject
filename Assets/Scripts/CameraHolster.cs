using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class CameraHolster : MonoBehaviour
{
	private Transform[] waypoints = new Transform[2];

	public Transform upTransform;
	public Transform holsteredTransform;
	public float speed = 8;

	float dstTravelled;
	VertexPath path;

	bool pathing = false;

	private void Start()
	{
		waypoints[1] = holsteredTransform;
	}


	public void GoUp()
	{
		pathing = true;
		MakePath(transform, upTransform);
	}

	public void Holster()
	{
		pathing = true;
		MakePath(transform, holsteredTransform);
	}

	void MakePath(Transform from, Transform to)
	{
		waypoints[0] = from;
		waypoints[1] = to;

		BezierPath bezierPath = new BezierPath(waypoints, false, PathSpace.xyz);
		path = new VertexPath(bezierPath);
	}

	void Update()
	{
		if (!pathing)
		{
			transform.position = waypoints[1].position;
			transform.rotation = waypoints[1].rotation;

			return;
		}

		Vector3 pathEnd = path.vertices[path.vertices.Length - 1];
		Vector3 trueEnd = waypoints[1].position;
		if (Vector3.Distance(transform.position, trueEnd) < 0.1f)
		{
			pathing = false;
			return;
		}

		if (Vector3.Distance(pathEnd, trueEnd) > 0.1f)
		{
			MakePath(waypoints[1], transform);
		}

		if (path != null)
		{
			dstTravelled += speed * Time.deltaTime;
			transform.position = path.GetPointAtDistance(dstTravelled);
		}
	}
}
