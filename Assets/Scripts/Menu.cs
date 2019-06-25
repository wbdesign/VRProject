using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	public bool menuOpen = false;
	public bool galleryOpen = false;

	public GameObject galleryCanvas;
	public GameObject menuCanvas;

	public void Start()
	{
		galleryCanvas.SetActive(false);
		menuCanvas.SetActive(false);
	}

	public void MenuQuit()
	{
		Application.Quit();
	}

	public void MenuStart()
	{
		SceneManager.LoadScene(1);
		Time.timeScale = 1f;
	}

	public void MenuGalleryOpen()
	{
		// Turn on Gallery Canvas
		galleryCanvas.SetActive(true);

		// Activate Gallery Shit
		galleryOpen = true;
	}

	public void MenuGalleryClose()
	{
		// Turn off Gallery Canvas
		galleryCanvas.SetActive(false);

		// Deactivate Gallery Shit
		galleryOpen = false;
	}

	public void MenuBack()
	{
		SceneManager.LoadScene(0);
		Time.timeScale = 1f;
	}

	public void Close()
	{
		// Close Menu
		menuCanvas.SetActive(false);
		menuOpen = false;
		Time.timeScale = 1f;
	}

	public void Open()
	{
		// Open Menu
		menuCanvas.SetActive(true);
		menuOpen = true;
		Time.timeScale = 0f;
	}

	public void ToggleMenu()
	{
		if (menuOpen)
			Close();
		else
			Open();
	}
}
