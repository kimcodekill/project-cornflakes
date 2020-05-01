using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointSystem : MonoBehaviour {

	/// <summary>
	/// Should saved checkpoints be discarded on next scene load?
	/// </summary>
	public static bool NewGame { get; set; }

	private static bool registered;

	private void OnEnable() {
		if (!registered) {
			SceneManager.activeSceneChanged += OnSceneChanged;
			SceneManager.sceneLoaded += OnSceneLoaded;
			registered = true;
		}
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.X)) CaptureKeeper.ClearCaptures();
	}

	private void OnSceneChanged(Scene prev, Scene next) {
		if (prev.name != null || NewGame) {
			NewGame = false;
			CaptureKeeper.ClearCaptures();
		}
	}

	private void OnSceneLoaded(Scene s, LoadSceneMode lsm) {
		CaptureKeeper.LoadLatestCapture();
	}

}