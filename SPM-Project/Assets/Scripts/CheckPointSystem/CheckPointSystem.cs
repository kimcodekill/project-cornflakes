using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Author: Viktor Dahlberg
public class CheckPointSystem : MonoBehaviour {

	/// <summary>
	/// Should saved checkpoints be discarded on next scene load?
	/// </summary>
	public static bool NewGame { get; set; }

	private static bool registered;

	private static int lastSceneBuildIndex;

	private void OnEnable() {
		if (!registered) {
			SceneManager.sceneUnloaded += OnSceneUnloaded;
			SceneManager.activeSceneChanged += OnSceneChanged;
			SceneManager.sceneLoaded += OnSceneLoaded;
			registered = true;
		}
	}

	private void OnSceneUnloaded(Scene scene) {
		lastSceneBuildIndex = scene.buildIndex;
	}

	private void OnSceneChanged(Scene prev, Scene next) {
		if (NewGame) {
			NewGame = false;
			CaptureKeeper.ClearCaptures();
		}
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode lsm) {
		if (lastSceneBuildIndex != scene.buildIndex) CaptureKeeper.NewLevel = true;
		CaptureKeeper.LoadLatestCapture();
	}

}