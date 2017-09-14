using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestureObject : MonoBehaviour {
	public Sprite[] spriteList;
	public string name;

	// Use this for initialization
	void Start () {
		Image image = gameObject.GetComponent<Image> ();
		image.sprite = GetRandomGesture ();
		name = image.sprite.name;
	}

	/// <summary>
	/// Gets a random gesture sprite.
	/// </summary>
	/// <returns>A random gesture sprite.</returns>
	private Sprite GetRandomGesture() {
		return spriteList[Random.Range (0, spriteList.Length)];
	}
}
