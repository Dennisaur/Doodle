using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour {

	private Theme theme;

	private bool blinkSpriteChanged;
	public float blinkLength;
	private float blinkTime;

	public Sprite pokeSprite;
	private GameObject hand;
	private float rotationTime;

	public Vector3 direction = Vector3.down;
	public float speed = 5f;

	// Use this for initialization
	void Start () {
		theme =  ThemeManager.instance.GetCurrentTheme();
		GetComponent<SpriteRenderer> ().sprite = theme.animalSprite;
		blinkTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		// Normal movement
		if (blinkTime == 0) {
			gameObject.transform.Translate (direction * Time.deltaTime * speed);
		} 
		// Destroy hand and animal objects after blink complete
		else if (Time.time > blinkTime + blinkLength) {
			Destroy (hand);
			Destroy (gameObject);
		}
		// Lerp hand transform during blink
		else if (hand) {
			hand.transform.rotation = Quaternion.Lerp (Quaternion.Euler (0, 0, 0), Quaternion.Euler (-20, 0, 0), (Time.time - blinkTime) * blinkLength);
		}
	}

	/// <summary>
	/// Raises the trigger enter 2d event.
	/// </summary>
	/// <param name="other">Other collider</param>
	void OnTriggerEnter2D(Collider2D other) {
		// Collision with hand
		if (other.gameObject.tag == "Bullet") {
			// Add to score
			GameManager.instance.AddPoints (1);

			// Set blink time and change to blinking sprite
			blinkTime = Time.time;
			GetComponent<SpriteRenderer> ().sprite = theme.animalBlinkSprite;

			// Sets reference to destroy later (after blink complete)
			hand = other.gameObject;
		}
		// Collision with player
		else if (other.gameObject.tag == "Player") {
			GameManager.instance.GameOver ();
		}
	}
}
