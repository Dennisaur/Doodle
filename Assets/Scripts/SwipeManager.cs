using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeManager : MonoBehaviour {
	private Theme[] themes;

	// Use this for initialization
	void Start () {
		themes = ThemeManager.instance.GetThemes ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
