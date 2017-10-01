using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilt : MonoBehaviour {
	public float period;
	public float angle;

	private float currentTime;

	// Use this for initialization
	void Start () {
		currentTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		currentTime += Time.deltaTime;
		float phase = Mathf.Sin (currentTime / period);
		transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, angle * phase));
	}
}
