using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureSpawner : MonoBehaviour {

	public GameObject gameObject;
	public GameObject[] spawnLocations;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void Spawn(int index) {
		Instantiate(gameObject, spawnLocations[index].transform.localPosition, Quaternion.identity);
	}

}
