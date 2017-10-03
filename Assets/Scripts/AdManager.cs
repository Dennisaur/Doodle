using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour {
	public static AdManager instance = null;

	public int playsPerAd;
	private int plays;


	// Called before Start functions
	void Awake () {
		// Check if instance already exists
		if (instance == null) {
			// If not, set instance to this
			instance = this;
		}
		// If instance already exists and it's not this:
		else if (instance != this) {
			// Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instane of GameManager
			Destroy (gameObject);
		}

		DontDestroyOnLoad (instance);
	}

	// Use this for initialization
	void Start () {
		plays = playsPerAd;
	}

	/// <summary>
	/// Decrements play counter and shows ad after given number of plays
	/// </summary>
	public void CompletePlay() {
		if (--plays <= 0) {
			ShowAd ();
			plays = playsPerAd;
		}
	}

	/// <summary>
	/// Shows the ad.
	/// </summary>
	public void ShowAd(string integrationId = "") {
		if (Advertisement.IsReady ()) {
			Advertisement.Show (integrationId, new ShowOptions (){ resultCallback = HandleAdResult });
		}
	}

	/// <summary>
	/// Handles the ad result.
	/// </summary>
	/// <param name="result">Result.</param>
	private void HandleAdResult(ShowResult result) {
		switch (result) {
		case ShowResult.Finished:
			Debug.Log ("Ad finished");
			break;
		case ShowResult.Skipped:
			Debug.Log ("Ad skipped");
			break;
		case ShowResult.Failed:
			Debug.Log ("Ad failed");
			break;
		}
	}
}
