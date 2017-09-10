using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestureObject : MonoBehaviour {
//	public Dictionary<string, Sprite> spriteList = new Dictionary<string, Sprite>();
	public Sprite[] spriteList;
	public string name;

	// Use this for initialization
	void Start () {
		Image image = gameObject.GetComponent<Image> ();
		image.sprite = GetRandomGesture ();
		name = image.sprite.name;
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}

	private Sprite GetRandomGesture() {
		return spriteList[Random.Range (0, spriteList.Length)];
	}
}
