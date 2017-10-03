using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
	public RectTransform UIcanvas;
	private float canvasWidth;

	public GameObject backToExit;
	public float backPressedDelay;
	private float backPressedTime;
	private bool backPressed;

	public CanvasGroup lockedGroup;
	public CanvasGroup unlockedGroup;

	public Text txtHighScore;

	// Game mode
	public Text txtGameMode;
	public GameObject autoObject;
	public GameObject tiltObject;
	public Text txtMultipler;

	public Text txtCurrency;
	public Text txtUnlockScore;
	public GameObject lockButton;
	public GameObject unlockButton;
	private bool unlocking;

	// Sound
	public GameObject soundOnSprite;
	public GameObject soundOffSprite;

	// Themes
	public GameObject scrollableBG;
	public GameObject prevTheme;
	public GameObject nextTheme;

	public float transitionTime;
	private float transitionStart;
	private bool lerpComplete;

	private Vector3 scrollableStartPosition;
	private Vector3 scrollableEndPosition;
	private Color colorStart;
	private Color colorEnd;

	private List<Image> dynamicImages;
	private List<Text> dynamicTexts;
	public Transform[] ignoreColorChange;

	public GameObject[] bgThemes;

	private Theme oldTheme;
	private Theme targetTheme;

	// Use this for initialization
	void Start () {
		// Use canvas width to set up scrollable background themes
		canvasWidth = UIcanvas.rect.width;
		for (int i = 0; i < bgThemes.Length; i++) {
			GameObject bgTheme = bgThemes [i];
			bgTheme.transform.localPosition = new Vector3 (canvasWidth * i, 0);
		}
		scrollableBG.transform.localPosition = new Vector3 (-canvasWidth * ThemeManager.instance.themeIndex, 0);

		// Get dynamic color texts
		dynamicTexts = new List<Text> ();
		Text[] texts = FindObjectsOfType<Text> ();
		bool ignore;
		foreach (Text text in texts) {
			ignore = false;
			foreach (Transform ignoreParent in ignoreColorChange) {
				if (text.transform.IsChildOf (ignoreParent)) {
					ignore = true;
					break;
				}
			}
			if (!ignore) {
				dynamicTexts.Add (text);
			}					
		}

		// Get dynamic color images
		dynamicImages = new List<Image> ();
		Image[] images = FindObjectsOfType<Image> ();
		foreach (Image image in images) {
			ignore = false;
			foreach (Transform ignoreParent in ignoreColorChange) {
				if (image.transform.IsChildOf (ignoreParent)) {
					ignore = true;
					break;
				}
			}
			if (!ignore) {
				dynamicImages.Add (image);
			}					
		}

		targetTheme = ThemeManager.instance.GetCurrentTheme ();
		oldTheme = targetTheme;
		lockedGroup.alpha = (!targetTheme.unlocked) ? 1 : 0;
		unlockedGroup.alpha = (targetTheme.unlocked) ? 1 : 0;

		SetUpThemeTransition();
		lerpComplete = true;
		UpdateMenu ();
	}

	// Save player prefs when pausing
	void OnApplicationPause(bool pauseStatus) {
		if (pauseStatus) {
			PlayerPrefs.Save ();
		}
	}

	/// <summary>
	/// When back button is pressed
	/// </summary>
	void Back() {
		if (backPressed) {
			PlayerPrefs.Save ();
			Application.Quit ();
		} else {
			backPressed = true;
			backPressedTime = Time.time;
			backToExit.SetActive (true);
		}
	}

	// Update is called once per frame
	void Update () {
		// Handle back button
		if (backPressed && Time.time - backPressedTime >= backPressedDelay) {
			backPressed = false;
			backToExit.SetActive (false);
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Back ();
		}

		float lerpPercent = (Time.time - transitionStart) / transitionTime;

		if (unlocking) {
			// Fade out locked object when unlocking theme
			lockedGroup.alpha = 1 - lerpPercent;
			unlockedGroup.alpha = lerpPercent;

			if (lerpPercent > 1) {
				unlocking = false;
				lockedGroup.gameObject.SetActive (!targetTheme.unlocked);
				unlockedGroup.gameObject.SetActive (targetTheme.unlocked);
			}
		}
		else if (!lerpComplete) {
			// Scroll background theme
			scrollableBG.transform.localPosition = Vector3.Lerp (scrollableStartPosition, scrollableEndPosition, lerpPercent);

			// Change text colors
			foreach (Text text in dynamicTexts) {
				text.color = Color.Lerp (colorStart, colorEnd, lerpPercent);
			}

			// Change image colors
			foreach (Image image in dynamicImages) {
				image.color = Color.Lerp (colorStart, colorEnd, lerpPercent);
			}

			// Fade locked/unlocked objects
			if (oldTheme.unlocked != targetTheme.unlocked) {
				if (targetTheme.unlocked) {
					lockedGroup.alpha = 1 - lerpPercent;
					unlockedGroup.alpha = lerpPercent;
				} else {
					lockedGroup.alpha = lerpPercent;
					unlockedGroup.alpha = 1 - lerpPercent;
				}
			}

			// When lerping is complete
			if (lerpPercent > 1) {
				lerpComplete = true;
				oldTheme = targetTheme;

				lockedGroup.gameObject.SetActive (!targetTheme.unlocked);
				unlockedGroup.gameObject.SetActive (targetTheme.unlocked);
			}
		}
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
	/// Unlocks the theme.
	/// </summary>
	public void UnlockTheme() {
		// Unlock theme in theme manager
		ThemeManager.instance.UnlockTheme();

		unlocking = true;
		transitionStart = Time.time;

		// Subtract cost from currency
		PlayerPrefs.SetInt ("Currency", PlayerPrefs.GetInt ("Currency") - targetTheme.unlockScore);

		UpdateMenu ();
	}

	/// <summary>
	/// Toggles the game mode.
	/// </summary>
	public void ToggleGameMode() {
		ThemeManager.instance.ToggleGameMode ();
		UpdateMenu ();
	}

	/// <summary>
	/// Updates the menu screen
	/// </summary>
	public void UpdateMenu() {
		targetTheme =  ThemeManager.instance.GetCurrentTheme();

		SetUpThemeTransition();

		// Set sound off/on
		bool soundIsOn = BackgroundMusic.instance.GetSoundIsOn ();
		soundOnSprite.SetActive (soundIsOn);
		soundOffSprite.SetActive (!soundIsOn);

		// Update currency
		int currency = PlayerPrefs.GetInt("Currency", 0);
		txtCurrency.text = currency.ToString();

		// Update game mode
		bool isTilt = ThemeManager.instance.GetGameMode() == GameMode.Tilt;
		txtGameMode.text = (isTilt) ? "Tilt" : "Auto";
		tiltObject.SetActive (isTilt);
		autoObject.SetActive (!isTilt);
		txtMultipler.text = "x" + (isTilt ? ThemeManager.instance.tiltMultiplier : 1); //TODO theme manager multiplier

		// Set menu settings when theme is locked
		if (!targetTheme.unlocked) {
			txtUnlockScore.gameObject.SetActive (!targetTheme.unlocked);

			// Set high score/unlock cost text
			int unlockCost = targetTheme.unlockScore;
			txtHighScore.text = PlayerPrefs.GetInt ("HighScore", 0).ToString ();
			txtUnlockScore.text = unlockCost.ToString ();

			// Set buttons if unlockable
			bool unlockable = (currency >= unlockCost);
			lockButton.SetActive (!unlockable);
			unlockButton.SetActive (unlockable);
		}
	}

	/// <summary>
	/// Updates the text colors based on theme.
	/// </summary>
	public void SetUpThemeTransition() {
		// Set variables for theme transition
		int themeIndex = ThemeManager.instance.themeIndex;
		transitionStart = Time.time;
		lerpComplete = false;

		// Get starting and target colors
		colorStart = oldTheme.textColor;
		colorEnd = targetTheme.textColor;

		// Get starting and target background position
		scrollableStartPosition = scrollableBG.transform.localPosition;
		scrollableEndPosition = new Vector3 (-canvasWidth * themeIndex, 0);

		// Disable next/prev if edge index
		prevTheme.SetActive (themeIndex > 0);
		nextTheme.SetActive (themeIndex < bgThemes.Length - 1);

		// Set locked/unlocked groups to active for fade transition
		lockedGroup.gameObject.SetActive (true);
		unlockedGroup.gameObject.SetActive (true);
	}

	public void DebugMe() {
		Debug.Log ("Currency: " + PlayerPrefs.GetInt ("Currency", -1));
		Debug.Log ("GameMode: " + PlayerPrefs.GetInt ("GameMode", -1));
		Debug.Log ("SoundOn: " + PlayerPrefs.GetInt ("SoundOn", -1));
		Debug.Log ("Theme: " + PlayerPrefs.GetInt ("ThemeIndex", -1));
	}
}
