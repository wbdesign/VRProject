using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Swim : MonoBehaviour
{
	public PathCreator pathCreator;
	public EndOfPathInstruction endOfPathInstruction;
	public float speed = 5;
	float distanceTravelled;

	[Header("Random Speeds")]
	public bool randomSpeeds = false;
	[Range(0.01f, 10.0f)]
	public float minSpeed = 1.0f;
	[Range(0.01f, 10.0f)]
	public float maxSpeed = 10.0f;
	[Range(0.1f, 2.0f)]
	public float lerpTime = 1.0f;
	private float m_currentLerp = 0.0f;
	private float m_startSpeed = 5;

	[Range(0.1f, 1.0f)]
	public float changeChance = 0.5f;
	[Range(2.0f, 10.0f)]
	public float changeTime = 2.0f;
	private float targetSpeed = 5;

	void Start()
	{
		if (pathCreator != null)
		{
			// Subscribed to the pathUpdated event so that we're notified if the path changes during the game
			pathCreator.pathUpdated += OnPathChanged;
		}

		if (randomSpeeds)
		{
			StartCoroutine(RandomSpeedChanger());
		}
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
	}

	void Update()
	{
		if (randomSpeeds)
		{
			m_currentLerp += Time.deltaTime;
			if (m_currentLerp > lerpTime)
				m_currentLerp = lerpTime;
			speed = Mathf.Lerp(m_startSpeed, targetSpeed, m_currentLerp / lerpTime);
		}

		if (pathCreator != null)
		{
			distanceTravelled += speed * Time.deltaTime;
			transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
			transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
		}
	}

	// If the path changes during the game, update the distance travelled so that the follower's position on the new path
	// is as close as possible to its position on the old path
	void OnPathChanged()
	{
		distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
	}


	//RANDOM SPEED CHANGE
	IEnumerator RandomSpeedChanger()
	{
		while (true)
		{
			yield return new WaitForSeconds(changeTime);

			float random = Random.value;
			if (random <= changeChance)
				targetSpeed = Random.Range(minSpeed, maxSpeed);
			m_startSpeed = speed;
			m_currentLerp = 0;
		}
	}
}
