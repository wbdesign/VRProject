using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Gallery : MonoBehaviour
{
	[SerializeField]
	Image image;
	string[] files = null;
	int whichScreenShotIsShown = 0;

	// Use this for initialization
	void Start()
	{
	}

	void GetPictureAndShowIt()
	{
		if (files.Length > 0)
		{
			string pathToFile = files[whichScreenShotIsShown];
			Texture2D texture = GetScreenshotImage(pathToFile);
			Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
				new Vector2(0.5f, 0.5f));
			image.sprite = sp;
		}
	}

	Texture2D GetScreenshotImage(string filePath)
	{
		Texture2D texture = null;
		byte[] fileBytes;
		if (File.Exists(filePath))
		{
			fileBytes = File.ReadAllBytes(filePath);
			texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
			texture.LoadImage(fileBytes);
		}
		return texture;
	}

	public void NextPicture()
	{
		files = Directory.GetFiles(Application.persistentDataPath + "/ScreenShot/", "*.png");
		if (files.Length > 0)
		{
			whichScreenShotIsShown += 1;
			if (whichScreenShotIsShown > files.Length - 1)
				whichScreenShotIsShown = 0;
			GetPictureAndShowIt();
		}
	}

	public void PreviousPicture()
	{
		files = Directory.GetFiles(Application.persistentDataPath + "/ScreenShot/", "*.png");
		if (files.Length > 0)
		{
			whichScreenShotIsShown -= 1;
			if (whichScreenShotIsShown < 0)
				whichScreenShotIsShown = files.Length - 1;
			GetPictureAndShowIt();
		}
	}

	public Texture2D GetLastScreenShot()
	{
		files = Directory.GetFiles(Application.persistentDataPath + "/ScreenShot/", "*.png");
		if (files.Length > 0)
			return GetScreenshotImage(files[files.Length - 1]);

		return null;
	}
}
