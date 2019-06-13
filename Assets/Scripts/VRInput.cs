using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VRInput : MonoBehaviour
{
	[Header("Trigger")]
	public UnityEvent eventTrigger;
	[Header("Trigger Down")]
	public UnityEvent eventTriggerDown;
	[Tooltip("Debug Key for trigger")]
	public KeyCode debugTrigger = KeyCode.Return;


	[Header("TrackPadClick")]
	public UnityEvent eventTrackClick;
	[Tooltip("Debug Key for track pad click")]
	public KeyCode debugTrackClick = KeyCode.RightShift;

	[Header("Back")]
	public UnityEvent eventBack;
	[Tooltip("Debug Key for Back")]
	public KeyCode debugBack = KeyCode.Backspace;

	// Start is called before the first frame update
	void Awake()
    {
		if (eventTrigger == null)
			eventTrigger = new UnityEvent();

		if (eventTriggerDown == null)
			eventTriggerDown = new UnityEvent();

		if (eventTrackClick == null)
			eventTrackClick = new UnityEvent();

		if (eventBack == null)
			eventBack = new UnityEvent();
    }

	private void OnDestroy()
	{
		StopAllCoroutines();
	}

	// Update is called once per frame
	void Update()
	{
		// Get Trigger Input
		bool inputTrigger = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
		inputTrigger |= Input.GetKey(debugTrigger);

		if (inputTrigger)
			eventTrigger.Invoke();

		bool inputTriggerDown = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
		inputTriggerDown |= Input.GetKeyDown(debugTrigger);

		if (inputTriggerDown)
			eventTriggerDown.Invoke();

		// Get Track Pad Input
		bool inputTrackClick = OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad);
		inputTrackClick |= Input.GetKeyDown(debugTrackClick);

		if (inputTrackClick)
			eventTrackClick.Invoke();

		// Get Back Input
		bool inputBackClick = OVRInput.GetDown(OVRInput.Button.Back);
		inputBackClick |= Input.GetKeyDown(debugBack);

		if (inputBackClick)
			eventBack.Invoke();
	}
}
