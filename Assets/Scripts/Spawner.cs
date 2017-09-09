using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject gameObject;
	public float spawnRate;
	public GameObject[] spawnLocations;

	private float lastSpawn;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// Spawn every spawnRate seconds
		if (Time.time - spawnRate >= lastSpawn) {
			// Spawn
			Instantiate(gameObject, spawnLocations[Random.Range(0, spawnLocations.Length)].transform.localPosition, Quaternion.identity);

			// Reset last spawn time
			lastSpawn = Time.time;
		}
	}
}
