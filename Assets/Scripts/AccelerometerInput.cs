using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class AccelerometerInput : MonoBehaviour {
	public Transform leftAnimalSpawn;
	public Transform rightAnimalSpawn;

	private Vector3 leftLane;
	private Vector3 midLane;
	private Vector3 rightLane;

	private Vector3 ogPosition;
	private Vector3 targetPosition;

	public float speed = 10f;
	public float tiltSensitivity;
	private bool lerping = false;

	private float startTime = 0;

	// Use this for initialization
	void Start () {
		leftLane = new Vector3 (leftAnimalSpawn.position.x, transform.position.y);
		rightLane = new Vector3 (rightAnimalSpawn.position.x, transform.position.y);
		midLane = new Vector3 (0, transform.position.y);

		targetPosition = midLane;

		TiltInput tiltInput = GetComponent<TiltInput> ();
		tiltInput.fullTiltAngle = 10 + (5 * (5 - tiltSensitivity));
	}

	// Update is called once per frame
	void Update () {
		if (CrossPlatformInputManager.GetAxis("Vertical") <= -1) {
			LerpPosition (leftLane);
		} else if (CrossPlatformInputManager.GetAxis("Vertical") >=	1) {
			LerpPosition (rightLane);
		} else {
			LerpPosition (midLane);
		}
//			if (Input.acceleration.x < -(tiltSensitivity)) {
//				LerpPosition (leftLane);
//			} else if (Input.acceleration.x > tiltSensitivity) {
//				LerpPosition (rightLane);
//			} else {
//				LerpPosition (midLane);
//			}
	}

	/// <summary>
	/// Lerps transform towards destination
	/// </summary>
	/// <param name="targetLane">New destination to lerp to.</param>
	void LerpPosition (Vector3 targetLane) {
		// Reset time and position when changing destination
		if (targetPosition != targetLane) {
			startTime = Time.time;
			ogPosition = transform.position;
			targetPosition = targetLane;
			lerping = true;
		}

		// Use journey distance and speed to lerp player position
		float journeyLength = Vector3.Distance(ogPosition, targetPosition);
		if (journeyLength <= 0) {
			return;
		}
		float distanceCovered = (Time.time - startTime) * speed;
		float fractionOfJourney = distanceCovered / journeyLength;
		transform.position = Vector3.Lerp (ogPosition, targetPosition, fractionOfJourney);

		// Lerp complete
		if (fractionOfJourney >= 1f) {
			lerping = false;
		}
	}
}
