using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoTime : MonoBehaviour
{
	// Player Movement
	PlayerMovement m_movement;

	// If PhotoTime is active
	public bool photoTime;

    /// <summary>
	/// Start
	/// </summary>
    void Start()
    {
		m_movement = FindObjectOfType<PlayerMovement>();
	}

    /// <summary>
	/// Update
	/// </summary>
    void Update()
    {
		// EARLY EXIT
		if (!photoTime)
			return;
    }

	/// <summary>
	/// StartPhotoTime
	/// </summary>
	public void StartPhotoTime()
	{
		photoTime = true;
	}

	/// <summary>
	/// ExitPhotoTime
	/// </summary>
	public void ExitPhotoTime()
	{
		photoTime = false;
	}
}
