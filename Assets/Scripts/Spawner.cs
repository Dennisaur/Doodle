using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject spawnObject;
	public GameObject spawnObjectParent;
	public float secondsPerSpawn;
	public float secondsPerLevel;
	public float spawnRateChange;

	public GameObject[] spawnLocations;

	private float lastSpawn;
	private float lastLevel;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

		// Spawn every spawnRate seconds
		if (Time.time - secondsPerSpawn >= lastSpawn) {
			// Spawn
			Instantiate(spawnObject, spawnLocations[Random.Range(0, spawnLocations.Length)].transform.position, Quaternion.identity, spawnObjectParent.transform);

			// Reset last spawn time
			lastSpawn = Time.time;
		}

		// Use rate change to adjust spawn rate per level
		if (Time.time - secondsPerLevel >= lastLevel) {
			secondsPerSpawn *= spawnRateChange;

			lastLevel = Time.time;
		}
	}
}
