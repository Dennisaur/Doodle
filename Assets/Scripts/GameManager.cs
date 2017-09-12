using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public static GameManager instance = null;

	public int score;
	public Text scoreText;


	public GameObject animal;
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
		PlayAgain ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) && !GetIsPaused()) {
			Pause ();
		}
	}

	public bool GetIsPaused() {
		return Time.timeScale == 0;
	}

	public void PlayAgain() {
		score = 0;
		scoreText.text = score.ToString ();
		ClearField ();

		// Hide modals and resume time
		modalGameOver.SetActive (false);
		modalPause.SetActive (false);
		Time.timeScale = 1;
	}
		
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
	}

	public void Pause() {
		Time.timeScale = 0;
		modalPause.SetActive (true);
	}

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
	public void AddPoints(int points) {
		score += points;
		scoreText.text = score.ToString ();
	}
}
