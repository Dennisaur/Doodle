using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour {
	public static AnimalManager instance = null;

	public RectTransform UIcanvas;
	private float canvasWidth;

	// Animal spawning variables
	public GameObject animal;
	public GameObject animalParent;
	public float startingSpawnRate;
	public float spawnRateChange;
	public float secondsPerLevel;
	public GameObject[] spawnLocations;

	private List<float> animalXPositions;
	private List<GameObject> animals;

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
		canvasWidth = UIcanvas.rect.width;
		float splitWidth = canvasWidth / 3;
		for (int i = 0; i < spawnLocations.Length; i++) {
			spawnLocations [i].transform.localPosition = new Vector3 (splitWidth * (i - 1), spawnLocations [i].transform.localPosition.y);
		}

		animalXPositions = new List<float> ();
		animals = new List<GameObject> ();
		ResetSpawnRate ();
	}

	// Update is called once per frame
	void Update () {
		// Spawn an animal object at the given spawnRate
		if (Time.time - (1f / spawnRate) >= lastSpawn) {
			// Spawn the animal object
			Vector3 randomSpawnPosition = spawnLocations[Random.Range(0, spawnLocations.Length)].transform.position;
			GameObject newAnimal = Instantiate(animal, randomSpawnPosition, Quaternion.identity);
			animals.Add (newAnimal);
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
		animalXPositions = new List<float> ();
	}

	/// <summary>
	/// Shift first animal out of list
	/// </summary>
	public void AnimalsShift() {
		animalXPositions.RemoveAt (0);
		animals.RemoveAt (0);
	}

	/// <summary>
	/// Returns x position of first animal
	/// </summary>
	/// <returns>The first animal x.</returns>
	public float GetFirstAnimalX() {
		return (animalXPositions.Count > 0) ? animalXPositions [0] : Mathf.Infinity;
	}
}