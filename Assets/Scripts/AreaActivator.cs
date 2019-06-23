using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaActivator : MonoBehaviour
{
	[Tooltip("Distance to have active")]
	public float viewDistance;
	[Tooltip("Angle in degrees")]
	[Range(0f, 360f)]
	public float viewCone;
	private Flock[] flocks;

	public float disableTime = 10.0f;

	// Start is called before the first frame update
	void Start()
	{
		flocks = FindObjectsOfType<Flock>();
	}

	// Update is called once per frame
	void Update()
	{
		for (int i = 0; i < flocks.Length; ++i)
		{
			bool enabled = true;
			Vector3 pos = flocks[i].transform.position;

			// Check Angle
			float angle = GetAngleToPlayer(pos);
			enabled &= angle < Mathf.Deg2Rad * viewCone;

			// Check Distance
			float distance = Vector3.Distance(pos, transform.position);
			enabled &= distance <= viewDistance;

			if (enabled)
			{
				flocks[i].gameObject.SetActive(enabled);
				if (flocks[i].deactivating != null)
				{
					flocks[i].StopCoroutine(flocks[i].deactivating);
					flocks[i].deactivating = null;
				}
			}
			else
			{
				if (flocks[i].gameObject.activeSelf)
				{
					if (flocks[i].deactivating == null)
						flocks[i].deactivating = flocks[i].StartCoroutine(flocks[i].DisableAfter(disableTime));
				}
			}
		}
	}

	float GetAngleToPlayer(Vector3 targetPos)
	{
		Vector3 dif = (targetPos - transform.position).normalized;
		Vector3 dir = transform.forward;
		float dot = Vector3.Dot(dif, dir);

		return Mathf.Acos(dot);
	}
}
