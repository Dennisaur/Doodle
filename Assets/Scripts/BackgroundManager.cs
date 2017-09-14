using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {
	private Theme theme;

	// Use this for initialization
	void Start () {
		UpdateBackground ();
	}

	/// <summary>
	/// Updates the background using theme stored in player prefs
	/// </summary>
	public void UpdateBackground() {
		theme = ThemeManager.instance.GetCurrentTheme();
		GetComponent<SpriteRenderer> ().sprite = theme.background;
	}
}
