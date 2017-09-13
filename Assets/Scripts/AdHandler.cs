using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdHandler : MonoBehaviour {

	public void ShowAd() {
		if (Advertisement.IsReady ()) {
			Advertisement.Show ();
		}
	}

	private void HandleAdResult(ShowResult result) {
		switch (result) {
		case ShowResult.Finished:
			break;
		case ShowResult.Skipped:
			break;
		case ShowResult.Failed:
			break;
		}
	}
}
