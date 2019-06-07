using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoTime : MonoBehaviour
{
	// Player Movement
	PlayerMovement m_movement;

	static string m_shotCountName = "ShotCount";

	// Photo Camera
	public Camera photoCamera;
	public RenderTexture texture;
	public Material material;

	// If PhotoTime is active
	public bool photoTime = false;

	private Texture2D[] textures; 

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

		// Take Photo
		if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
		{
			TakePhoto();
			ExitPhotoTime();
		}

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

	/// <summary>
	/// TakePhoto
	/// </summary>
	public void TakePhoto()
	{
		if (photoTime)
		{
			m_movement.isDisabled = true;
			StartCoroutine(TakeScreenShot());

			ExitPhotoTime();
		}
	}

	private IEnumerator TakeScreenShot()
	{
		yield return new WaitForEndOfFrame();

		Texture2D screenShot = new Texture2D(texture.width, texture.height, TextureFormat.RGB24, false);
		photoCamera.Render();
		RenderTexture.active = texture;
		screenShot.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
		screenShot.Apply();

		int shotCount = PlayerPrefs.GetInt(m_shotCountName);

		string fileName ="Resources/ScreenShots/" + shotCount.ToString() + ".png";

		byte[] bytes = screenShot.EncodeToPNG();
		System.IO.File.WriteAllBytes(fileName, bytes);

		Debug.Log("Took ScreenShot");
		PlayerPrefs.SetInt(m_shotCountName, shotCount + 1);
		m_movement.isDisabled = false;

		textures = Resources.LoadAll<Texture2D>("ScreenShots");
		Debug.LogWarning(textures.Length.ToString());
	}
}
