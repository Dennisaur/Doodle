using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour {
	public static ThemeManager instance = null;

	public int tiltMultiplier = 2;

	public int themeIndex;
	public Theme[] defaultThemes;
	private Theme[] themes;
	private int highScore;

	private GameMode gameMode;

	// Called before Start functions
	void Awake () {
		// Check if instance already exists
		if (instance == null) {
			// If not, set instance to this
			instance = this;

			// Initialize playerprefs in awake because other objects depends on theme to be intialized
			string temp = "";
			themes = new Theme[defaultThemes.Length];
			for (int i = 0; i < defaultThemes.Length; ++i) {
				temp = PlayerPrefs.GetString ("Theme-" + i, "");
				if (temp == "") {
					themes [i] = defaultThemes [i];
				} else {
					themes [i] = JsonUtility.FromJson<Theme> (temp);
				}
			}

			themeIndex = PlayerPrefs.GetInt ("ThemeIndex", 0);
			highScore = PlayerPrefs.GetInt ("HighScore", 0);
			gameMode = (GameMode)PlayerPrefs.GetInt ("GameMode", 0);
		}
		// If instance already exists and it's not this:
		else if (instance != this) {
			// Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instane of GameManager
			Destroy (gameObject);
		}

		DontDestroyOnLoad (instance);
	}

	// Use this for initialization
	void Start() {
	}

	/// <summary>
	/// Switch to previous theme
	/// </summary>
	public void PreviousTheme() {
		themeIndex = (--themeIndex + themes.Length) % themes.Length;
		PlayerPrefs.SetInt ("ThemeIndex", themeIndex);
	}

	/// <summary>
	/// Switch to next theme
	/// </summary>
	public void NextTheme() {
		themeIndex = ++themeIndex % themes.Length;
		PlayerPrefs.SetInt ("ThemeIndex", themeIndex);
	}

	/// <summary>
	/// Returns the current theme
	/// </summary>
	/// <returns>The current theme.</returns>
	public Theme GetCurrentTheme() {
		return themes [themeIndex];
	}

	/// <summary>
	/// Unlocks the theme at a given index
	/// </summary>
	/// <param name="index">Index.</param>
	public void UnlockTheme() {
		Theme theme = themes [themeIndex];
		theme.unlocked = true;
		PlayerPrefs.SetString ("Theme-" + themeIndex, JsonUtility.ToJson (theme));
		PlayerPrefs.Save ();
	}

	/// <summary>
	/// Toggles the game mode.
	/// </summary>
	public void ToggleGameMode() {
		if (gameMode == GameMode.Auto) {
			gameMode = GameMode.Tilt;
		} else {
			gameMode = GameMode.Auto;
		}

		PlayerPrefs.SetInt ("GameMode", (int)gameMode);
		PlayerPrefs.Save ();
	}

	/// <summary>
	/// Gets the themes.
	/// </summary>
	/// <returns>The themes.</returns>
	public Theme[] GetThemes() {
		return themes;
	}

	/// <summary>
	/// Gets the game mode.
	/// </summary>
	/// <returns>The game mode.</returns>
	public GameMode GetGameMode() {
		return gameMode;
	}
}

// Used for setting up dictionary in editor
[System.Serializable]
public class Theme {
	public string name;
	public Sprite background;
	public Sprite animalSprite;
	public Sprite animalBlinkSprite;
	public int unlockScore;
	public bool unlocked;
	public Color textColor;
}