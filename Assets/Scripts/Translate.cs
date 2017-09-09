using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translate : MonoBehaviour {
	public Vector3 direction = Vector3.down;
	public float speed = 5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Translate(direction * Time.deltaTime * speed);

		if (gameObject.transform.localPosition.y <= -11 || gameObject.transform.localPosition.y > 9) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (gameObject.name == "Triangle(Clone)") {
			Destroy (gameObject);
			Destroy (other.gameObject);
		}
	}

}
