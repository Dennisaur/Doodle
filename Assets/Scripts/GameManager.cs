﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class GameManager : MonoBehaviour {
	public static GameManager instance = null;
	public GestureHandlerQueue gestureHandler;

	public GameMode gameMode;

	private int points;
	public int score;
	public Text scoreText;

	public GameObject player;

	public GameObject animal;
	public AnimalManager animalManager;
	public GameObject hand;

	public GameObject modalPause;
	public GameObject modalGameOver;
	public Text gameOverText;
	public Text gameOverScore;
	public Text gameOverHighScore;

	private int highScore;

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
	}

	// Use this for initialization
	void Start () {
		highScore = PlayerPrefs.GetInt ("HighScore", 0);
		SetGameMode ();
		PlayAgain ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) && !GetIsPaused()) {
			Pause ();
		}
	}

	private void SetGameMode() {
		gameMode = (GameMode)PlayerPrefs.GetInt ("GameMode", 0);
		switch (gameMode) {
		case GameMode.Auto:
			points = 1;
			break;
		case GameMode.Stick:
			player.GetComponent<Joystick> ().enabled = true;
			break;
		case GameMode.Tilt:
			player.GetComponent<AccelerometerInput> ().enabled = true;
			points = ThemeManager.instance.tiltMultiplier;
			break;
		}
	}

	/// <summary>
	/// Return true if paused
	/// </summary>
	/// <returns><c>true</c>, if game is currently paused, <c>false</c> otherwise.</returns>
	public bool GetIsPaused() {
		return Time.timeScale == 0;
	}

	/// <summary>
	/// Restarts the game
	/// </summary>
	public void PlayAgain() {
		score = 0;
		scoreText.text = score.ToString ();
		ClearField ();
		animalManager.ResetSpawnRate ();
		gestureHandler.ResetGestures ();

		// Hide modals and resume time
		modalGameOver.SetActive (false);
		modalPause.SetActive (false);
		Time.timeScale = 1;
	}

	/// <summary>
	/// Displays game over screen
	/// </summary>
	public void GameOver() {
		// Pause the game
		Time.timeScale = 0;

		if (score > highScore) {
			gameOverText.text = "New High Score!";
			highScore = score;
			PlayerPrefs.SetInt ("HighScore", highScore);
		} else {
			gameOverText.text = "Game Over";
		}
		gameOverScore.text = score.ToString ();
		gameOverHighScore.text = highScore.ToString ();
		modalGameOver.SetActive (true);

		ScoreToCurrency ();

		AdManager.instance.CompletePlay ();
	}

	public void ScoreToCurrency() {
		bool hasKey = PlayerPrefs.HasKey ("Currency");
		if (hasKey) {
			PlayerPrefs.SetInt ("Currency", score + PlayerPrefs.GetInt ("Currency", 0));
		} else {
			PlayerPrefs.SetInt ("Currency", score);
		}
		PlayerPrefs.Save ();
	}

	/// <summary>
	/// Pauses the game.
	/// </summary>
	public void Pause() {
		Time.timeScale = 0;
		modalPause.SetActive (true);
	}

	/// <summary>
	/// Resumes the game.
	/// </summary>
	public void Resume() {
		Time.timeScale = 1;
		modalPause.SetActive (false);
	}

	/// <summary>
	/// Clears the field of enemy and bullet objects
	/// </summary>
	void ClearField() {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		GameObject[] bullets = GameObject.FindGameObjectsWithTag ("Bullet");

		foreach (GameObject enemy in enemies) {
			Destroy (enemy);
		}
		foreach (GameObject bullet in bullets) {
			Destroy (bullet);
		}
	}

	/// <summary>
	/// Adds points to current score
	/// </summary>
	/// <param name="points">Points.</param>
	public void AddPoints() {
		score += points;
		scoreText.text = score.ToString ();
	}
}

public enum GameMode {
	Auto,
	Stick,
	Tilt
}