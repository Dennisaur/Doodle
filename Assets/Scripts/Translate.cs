using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour {
	public Vector3 direction = Vector3.down;
	public float speed = 5f;

	private bool stopMovement = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!stopMovement) {
			gameObject.transform.Translate (direction * Time.deltaTime * speed);
		}

		if (gameObject.transform.position.y > 8) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (gameObject.tag == "Bullet") {
			if (other.gameObject.tag == "Enemy") {
				stopMovement = true;
			}
		}
	}

}
