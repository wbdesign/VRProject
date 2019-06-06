using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
	enum MoveState
	{
		start = 0,
		ongoing,
		stop
	}
	// Current Movement State
	MoveState m_moveState = MoveState.stop;

	// Current Speed
	float m_speed = 0;
	float m_timer = 0;

	uint m_offFrames = 0;
	uint m_offDelay = 1;

	CharacterController character;

	[Tooltip("Time taken to ramp up from 0 - Max Speed (in seconds)")]
	[Range(0, 5)]
	public float rampUpTime = 0.5f;

	[Tooltip("Time taken to ramp down from Max Speed - 0 (in seconds)")]
	[Range(0, 5)]
	public float rampDownTime = 1.0f;

	[Tooltip("Max Speed")]
	[Range(1,10)]
	public float maxSpeed = 1.0f;

	[Tooltip("Debug Key for testing")]
	public KeyCode debugKey = KeyCode.Return;

	private void Awake()
	{
		character = GetComponent<CharacterController>();
	}

	private void FixedUpdate()
	{
		// Get Input
		bool input = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
		input |= Input.GetKey(debugKey);

		// Setup Movement State
		if (input)
		{
			m_offFrames = 0;

			if (m_moveState == MoveState.stop)
			{
				m_moveState = MoveState.start;
				m_timer = 0;
			}
		}
		else
		{
			m_offFrames++;
			if (m_offFrames > m_offDelay)
				m_moveState = MoveState.stop;
		}

		// For starting
		if (m_moveState == MoveState.start)
		{
			m_timer += Time.fixedDeltaTime;
			if (m_timer > rampUpTime)
				m_moveState = MoveState.ongoing;

			m_speed = Mathf.Lerp(m_speed, maxSpeed, m_timer / rampUpTime);
		}

		// Ongoing Movement
		if (m_moveState == MoveState.ongoing)
		{
			m_speed = maxSpeed;
		}

		// Stopping
		if (m_moveState == MoveState.stop)
		{
			if (m_timer < rampDownTime)
				m_timer += Time.fixedDeltaTime;

			m_speed = Mathf.Lerp(m_speed, 0, m_timer / rampDownTime);
		}

		// Actually Move now
		character.SimpleMove(m_speed * Camera.current.transform.forward);
	}
}
