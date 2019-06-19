using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class PhotoTime : MonoBehaviour
{
	// Player Movement
	PlayerMovement m_movement;

	static string m_shotCountName = "ShotCount";

	// Photo Camera
	[Header("Camera Stuff")]
	public CameraHolster camHolster;
	public Camera photoCamera;
	public RenderTexture texture;
	public Material material;


	// If PhotoTime is active
	private bool photoTime = false;
	private bool takingShot = false;

	[Header("ScreenShot Image")]
	public Gallery gallery;
	public Image image;
	private bool displayShot = false;
	private bool imageShown = false;

	private Texture2D[] textures;

	static string screenShotDir = "/ScreenShot";

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

		// Bring Camera UP
		photoCamera.enabled = true;
		camHolster.GoUp();
	}

	/// <summary>
	/// ExitPhotoTime
	/// </summary>
	public void ExitPhotoTime()
	{
		photoTime = false;
		takingShot = false;

		// Put camera down
		photoCamera.enabled = false;
		camHolster.Holster();
	}

	/// <summary>
	/// TakePhoto
	/// </summary>
	public void TakePhoto()
	{
		if (photoTime)
		{
			m_movement.movementLock = 0.5f;
			if (!takingShot)
			{
				takingShot = true;
				StartCoroutine(TakeScreenShot());
			}
		}
	}

	/// <summary>
	/// ClosePhoto
	/// </summary>
	public void ClosePhoto()
	{
		if (photoTime)
		{
			displayShot = false;
			ExitPhotoTime();
		}
	}

	/// <summary>
	/// TakeScreenShot
	/// </summary>
	/// <returns></returns>
	private IEnumerator TakeScreenShot()
	{
		Texture2D screenShot = new Texture2D(texture.width, texture.height, TextureFormat.RGB24, false);
		photoCamera.Render();
		RenderTexture.active = texture;
		screenShot.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
		screenShot.Apply();

		int shotCount = PlayerPrefs.GetInt(m_shotCountName);

		string fileName = Application.persistentDataPath + screenShotDir + "/" + shotCount.ToString().PadLeft(4,'0') + ".png";

		byte[] bytes = screenShot.EncodeToPNG();
		System.IO.File.WriteAllBytes(fileName, bytes);

		Debug.Log("Took ScreenShot");
		PlayerPrefs.SetInt(m_shotCountName, shotCount + 1);

		yield return new WaitForSecondsRealtime(0.5f);
		displayShot = true;
	}

	/// <summary>
	/// Update
	/// </summary>
	private void Update()
	{
		if (displayShot)
		{
			ShowImage();
		}
		else
		{
			image.gameObject.SetActive(false);
			imageShown = false;
		}
	}

	/// <summary>
	/// ShowImage
	/// </summary>
	private void ShowImage()
	{
		if (imageShown)
			return;

		imageShown = true;
		image.gameObject.SetActive(true);

		Texture2D texture = gallery.GetLastScreenShot();
		Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
			new Vector2(0.5f, 0.5f));
		image.sprite = sp;
	}
}
