﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
	public Button playButton;
	public Button unlockButton;
	public Text txtHighScore;
	public GameObject unlockScore;
	public Text txtUnlockScore;

	public Image soundButtonImage;
	public GameObject soundOnSprite;
	public GameObject soundOffSprite;

	public BackgroundManager background;

	public GameObject[] animals;

	private Theme theme;

	// Use this for initialization
	void Start () {
		UpdateMenu ();
	}

	/// <summary>
	/// Switches to the previous theme and updates menu accordingly
	/// </summary>
	public void PreviousTheme() {
		ThemeManager.instance.PreviousTheme ();
		UpdateMenu ();
	}

	/// <summary>
	/// Switches to the next theme and updates menu accordingly
	/// </summary>
	public void NextTheme() {
		ThemeManager.instance.NextTheme ();
		UpdateMenu ();
	}

	/// <summary>
	/// Toggles the sound.
	/// </summary>
	public void ToggleSound() {
		BackgroundMusic.instance.ToggleSound ();
		UpdateMenu ();
	}

	/// <summary>
	/// Updates the menu screen
	/// </summary>
	public void UpdateMenu() {
		// Update background
		background.UpdateBackground ();

		// Set animal sprites
		theme =  ThemeManager.instance.GetCurrentTheme();
		foreach (GameObject animal in animals) {
			animal.GetComponent<SpriteRenderer> ().sprite = theme.animalSprite;
		}

		// Set play/unlock button
		playButton.gameObject.SetActive (theme.unlocked);
		unlockButton.gameObject.SetActive (!theme.unlocked);
		unlockButton.interactable = PlayerPrefs.GetInt ("HighScore", 0) >= theme.unlockScore;

		// Set high score/unlock score text
		txtHighScore.text = PlayerPrefs.GetInt ("HighScore", 0).ToString();
		unlockScore.SetActive (!theme.unlocked);
		txtUnlockScore.text = theme.unlockScore.ToString();

		// Set sound off/on
		bool soundIsOn = BackgroundMusic.instance.GetSoundIsOn ();
		soundOnSprite.SetActive (soundIsOn);
		soundOffSprite.SetActive (!soundIsOn);

		ThemeManager.instance.UpdateTheme ();
	}

	/// <summary>
	/// Unlocks the theme.
	/// </summary>
	public void UnlockTheme() {
		// Unlock theme in player prefs
		ThemeManager.instance.UnlockTheme();

		UpdateMenu ();
	}
}
