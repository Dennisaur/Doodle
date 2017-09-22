using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMusic : MonoBehaviour {
	public static BackgroundMusic instance = null;

	private bool soundIsOn;
	public AudioSource audioSource;

	// Called before Start functions
	void Awake () {
		// Check if instance already exists
		if (instance == null) {
			// If not, set instance to this
			instance = this;
			soundIsOn = (PlayerPrefs.GetInt ("SoundOn", 1) == 1);
			audioSource.mute = !soundIsOn;
		}
		// If instance already exists and it's not this:
		else if (instance != this) {
			// Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instane of GameManager
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
	}

	/// <summary>
	/// Getter function for if sound is on
	/// </summary>
	/// <returns><c>true</c>, if sound is on, <c>false</c> otherwise.</returns>
	public bool GetSoundIsOn() {
		return soundIsOn;
	}
		
	/// <summary>
	/// Toggles the sound 
	/// </summary>
	public void ToggleSound() {
		soundIsOn = !soundIsOn;
		PlayerPrefs.SetInt ("SoundOn", soundIsOn ? 1 : 0);
		audioSource.mute = !soundIsOn;
	}
}
