using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject spawnObject;
	public GameObject spawnObjectParent;
	public float startingSpawnRate;
	public float spawnRate;
	public float secondsPerLevel;
	public float spawnRateChange;

	public GameObject[] spawnLocations;

	private float lastSpawn;
	private float lastLevel;

	// Use this for initialization
	void Start () {
		ResetSpawnRate ();
	}

	// Update is called once per frame
	void Update () {

		// Spawn every spawnRate seconds
		if (Time.time - (1f / spawnRate) >= lastSpawn) {
			// Spawn
			Instantiate(spawnObject, spawnLocations[Random.Range(0, spawnLocations.Length)].transform.position, Quaternion.identity, spawnObjectParent.transform);

			// Reset last spawn time
			lastSpawn = Time.time;
		}

		// Use rate change to adjust spawn rate per level
		if (Time.time - secondsPerLevel >= lastLevel) {
			spawnRate *= spawnRateChange;

			lastLevel = Time.time;
		}
	}

	/// <summary>
	/// Resets the spawn rate
	/// </summary>
	public void ResetSpawnRate () {
		spawnRate = startingSpawnRate;
		lastSpawn = Time.time;
		lastLevel = Time.time;
	}
}
