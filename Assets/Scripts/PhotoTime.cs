using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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

	private bool takingShot = false;

	private Texture2D[] textures;

	private string screenShotDir = "/ScreenShot";

    /// <summary>
	/// Start
	/// </summary>
    void Start()
    {
		m_movement = FindObjectOfType<PlayerMovement>();
		if (!Directory.Exists(Application.persistentDataPath + screenShotDir))
			Directory.CreateDirectory(Application.persistentDataPath + screenShotDir);
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
			if (!takingShot)
			{
				takingShot = true;
				StartCoroutine(TakeScreenShot());
				m_movement.movementLock = 5.0f;
			}
		}
	}

	private IEnumerator TakeScreenShot()
	{
		Texture2D screenShot = new Texture2D(texture.width, texture.height, TextureFormat.RGB24, false);
		photoCamera.Render();
		RenderTexture.active = texture;
		screenShot.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
		screenShot.Apply();

		int shotCount = PlayerPrefs.GetInt(m_shotCountName);

		string fileName = Application.persistentDataPath + screenShotDir + "/" + shotCount.ToString() + ".png";

		byte[] bytes = screenShot.EncodeToPNG();
		System.IO.File.WriteAllBytes(fileName, bytes);

		Debug.Log("Took ScreenShot");
		PlayerPrefs.SetInt(m_shotCountName, shotCount + 1);

		ExitPhotoTime();

		yield return new WaitForSecondsRealtime(0.5f);
		takingShot = false;
	}
}
