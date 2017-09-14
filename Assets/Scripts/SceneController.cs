using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	/// <summary>
	/// Loads the scene.
	/// </summary>
	/// <param name="sceneName">Scene name.</param>
	public void LoadScene(string sceneName) {
		SceneManager.LoadScene (sceneName);
	}

}
