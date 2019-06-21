using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
	public Flock myFlock;

	public float speed = 0.001f;
	float rotationSpeed = 5.0f;
	float minSpeed = 0.8f;
	float maxSpeed = 2.0f;
	Vector3 averageHeading;
	Vector3 averagePosition;
	float neighbourDistance = 3.0f;
	public Vector3 newGoalPos;

	public bool turning = false;

	// Use this for initialization
	void Start()
	{
		ChangeSpeed(Random.Range(minSpeed, maxSpeed));		
	}

	void OnTriggerEnter(Collider other)
	{
		if (!turning)
		{
			newGoalPos = this.transform.position - other.gameObject.transform.position;
		}

		turning = true;

	}

	void OnTriggerExit(Collider other)
	{
		turning = false;
	}

	void ChangeSpeed(float newSpeed)
	{
		speed = newSpeed;

		// NEED TO FIGURE THIS OUT
		// not using animation
		//this.GetComponent<Animation>()["Motion"].speed = speed;
	}


	// Update is called once per frame
	void Update()
	{
		if (turning)
		{
			Vector3 direction = newGoalPos - transform.position;
			transform.rotation = Quaternion.Slerp(transform.rotation,
													  Quaternion.LookRotation(direction),
													  rotationSpeed * Time.deltaTime);
			ChangeSpeed(Random.Range(minSpeed, maxSpeed));
		}
		else
		{
			if (Random.Range(0, 10) < 1)
				ApplyRules();

		}
		transform.Translate(0, 0, Time.deltaTime * speed);
	}

	void ApplyRules()
	{
		List<GameObject> gos;
		gos = myFlock.allFish;

		Vector3 vcentre = Vector3.zero;
		Vector3 vavoid = Vector3.zero;
		float gSpeed = 0.1f;

		Vector3 goalPos = myFlock.goalPos;

		float dist;

		int groupSize = 0;
		foreach (GameObject go in gos)
		{
			if (go != this.gameObject)
			{
				dist = Vector3.Distance(go.transform.position, this.transform.position);
				if (dist <= neighbourDistance)
				{
					vcentre += go.transform.position;
					groupSize++;

					if (dist < 2.0f)
					{
						vavoid = vavoid + (this.transform.position - go.transform.position);
					}

					Boid otherFish = go.GetComponent<Boid>();
					gSpeed = gSpeed + otherFish.speed;
				}
			}
		}

		if (groupSize > 0)
		{
			vcentre = vcentre / groupSize + (goalPos - this.transform.position);
			ChangeSpeed(gSpeed / groupSize);

			Vector3 direction = (vcentre + vavoid) - transform.position;
			if (direction != Vector3.zero)
			{
				var targetRot = Quaternion.LookRotation(direction);
				float rotSpeed = rotationSpeed * Time.deltaTime;

				transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed);
			}
		}
	}
}
