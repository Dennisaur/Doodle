using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerometerInput : MonoBehaviour {
	public Transform leftAnimalSpawn;
	public Transform rightAnimalSpawn;

	private Vector3 leftDest;
	private Vector3 middleDest;
	private Vector3 rightDest;

	private Vector3 ogPosition;
	private Vector3 destPosition;

	public float speed = 10f;
	public float tiltSensitivity = 0.2f;
	private bool lerping = false;

	private float startTime = 0;

	// Use this for initialization
	void Start () {
		leftDest = new Vector3 (leftAnimalSpawn.position.x, transform.position.y);
		rightDest = new Vector3 (rightAnimalSpawn.position.x, transform.position.y);
		middleDest = new Vector3 (0, transform.position.y);

		destPosition = middleDest;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.acceleration.x < -(tiltSensitivity)) {
			LerpPosition (leftDest);
		} else if (Input.acceleration.x > tiltSensitivity) {
			LerpPosition (rightDest);
		} else {
			LerpPosition (middleDest);
		}
	}

	void LerpPosition (Vector3 newDest) {
		// Reset time and position when changing destination
		if (destPosition != newDest) {
			startTime = Time.time;
			ogPosition = transform.position;
			destPosition = newDest;
			lerping = true;
		}

		// Use journey distance and speed to lerp player position
		float journeyLength = Vector3.Distance(ogPosition, destPosition);
		if (journeyLength <= 0) {
			return;
		}
		float distanceCovered = (Time.time - startTime) * speed;
		float fractionOfJourney = distanceCovered / journeyLength;
		transform.position = Vector3.Lerp (ogPosition, destPosition, fractionOfJourney);

		// Lerp complete
		if (fractionOfJourney >= 1f) {
			lerping = false;
		}
	}
}
