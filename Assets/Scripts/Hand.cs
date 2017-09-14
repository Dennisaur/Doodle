using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {
	public Vector3 direction = Vector3.down;
	public float speed = 5f;

	private bool stopMovement = false;
	
	// Update is called once per frame
	void Update () {
		// Normal movement
		if (!stopMovement) {
			gameObject.transform.Translate (direction * Time.deltaTime * speed);
		}

		// Destroy when off screen
		if (gameObject.transform.position.y > 10) {
			Destroy (gameObject);
		}
	}

	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="other">Other collider</param>
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Enemy") {
			stopMovement = true;
		}
	}

}
