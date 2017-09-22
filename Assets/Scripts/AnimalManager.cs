using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour {
	public static AnimalManager instance = null;

	// Animal spawning variables
	public GameObject animal;
	public GameObject animalParent;
	public float startingSpawnRate;
	public float spawnRateChange;
	public float secondsPerLevel;
	public GameObject[] spawnLocations;

	private List<float> animalXPositions;

	private float spawnRate;
	private float lastSpawn;
	private float lastLevel;

	// Called before Start functions
	void Awake () {
		// Check if instance already exists
		if (instance == null) {
			// If not, set instance to this
			instance = this;
		}
		// If instance already exists and it's not this:
		else if (instance != this) {
			// Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instane of GameManager
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		animalXPositions = new List<float> ();
		ResetSpawnRate ();
	}

	// Update is called once per frame
	void Update () {
		// Spawn an animal object at the given spawnRate
		if (Time.time - (1f / spawnRate) >= lastSpawn) {
			// Spawn the animal object
			Vector3 randomSpawnPosition = spawnLocations[Random.Range(0, spawnLocations.Length)].transform.position;
			Instantiate(animal, randomSpawnPosition, Quaternion.identity, animalParent.transform);
			animalXPositions.Add (randomSpawnPosition.x);

			// Reset last spawn time
			lastSpawn = Time.time;
		}

		// Use rate change to adjust spawn rate per level
		if (Time.time - secondsPerLevel >= lastLevel) {
			// Adjust spawn rate
			spawnRate *= spawnRateChange;

			// Reset last level change time
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

	public void AnimalsShift() {
		animalXPositions.RemoveAt (0);
	}

	public float GetFirstAnimalX() {
		return (animalXPositions.Count > 0) ? animalXPositions [0] : Mathf.Infinity;
	}
}