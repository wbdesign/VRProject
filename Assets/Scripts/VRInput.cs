using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VRInput : MonoBehaviour
{
	[Header("Trigger")]
	public UnityEvent eventTrigger;
	[Tooltip("Debug Key for trigger")]
	public KeyCode debugTrigger = KeyCode.Return;

	[Header("TrackPadClick")]
	public UnityEvent eventTrackClick;
	[Tooltip("Debug Key for track pad click")]
	public KeyCode debugTrackClick = KeyCode.RightShift;

	// Start is called before the first frame update
	void Awake()
    {
		if (eventTrigger == null)
			eventTrigger = new UnityEvent();

		if (eventTrackClick == null)
			eventTrackClick = new UnityEvent();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		// Get Trigger Input
		bool inputTrigger = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
		inputTrigger |= Input.GetKey(debugTrigger);

		if (inputTrigger)
			eventTrigger.Invoke();

		// Get Track Pad Input
		bool inputTrackClick = OVRInput.Get(OVRInput.Button.PrimaryTouchpad);
		inputTrackClick |= Input.GetKey(debugTrackClick);

		if (inputTrackClick)
			eventTrackClick.Invoke();
	}
}
