using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointSystem : MonoBehaviour {

	public static bool NewLevel { get; set; }

	private static bool registered;

	private void OnEnable() {
		if (!registered) {
			SceneManager.activeSceneChanged += OnSceneChanged;
			SceneManager.sceneLoaded += OnSceneLoaded;
			registered = true;
		}
	}

	private void OnSceneChanged(Scene prev, Scene next) {
		if (prev.name != null) {
			CaptureKeeper.ClearCaptures();
		}
	}

	private void OnSceneLoaded(Scene s, LoadSceneMode lsm) {
		CaptureKeeper.LoadLatestCapture();
	}

}