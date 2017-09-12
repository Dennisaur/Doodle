using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour {
	public Sprite[] sprites;
	public Sprite blinkSprite;
	private bool blinkSpriteChanged;
	public float blinkLength;
	private float blinkTime = 0;
	private GameObject hand;

	public Vector3 direction = Vector3.down;
	public float speed = 5f;

	// Use this for initialization
	void Start () {
		// Set random animal sprite
		//GetComponent<SpriteRenderer> ().sprite = sprites [Random.Range (0, sprites.Length)];
	}
	
	// Update is called once per frame
	void Update () {
		if (blinkTime == 0) {
			gameObject.transform.Translate (direction * Time.deltaTime * speed);
		} else if (Time.time > blinkTime + blinkLength) {
			Destroy (gameObject);
			Destroy (hand);
			GameManager.instance.AddPoints (1);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Bullet") {
			blinkTime = Time.time;
			hand = other.gameObject;
			GetComponent<SpriteRenderer> ().sprite = blinkSprite;
		} else if (other.gameObject.tag == "Player") {
			GameManager.instance.GameOver ();
		}
	}
}
