using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointKeeper : MonoBehaviour {

	public static bool NewLevel { get; set; }

	private static bool registered;

	private static Dictionary<Vector3, bool> checkPoints;

	private void OnEnable() {
		if (!registered) {
			SceneManager.activeSceneChanged += OnSceneChanged;
			SceneManager.sceneLoaded += OnSceneLoaded;
			registered = true;
		}
	}

	private void OnSceneChanged(Scene prev, Scene next) {
		if (prev.name != null) {
			ResetCheckPoints();
		}
	}

	private void OnSceneLoaded(Scene s, LoadSceneMode lsm) {
		CaptureKeeper.LoadLatestCapture();
		LoadCheckPoints();
	}

	private static void LoadCheckPoints() {
		if (checkPoints == null) return;
		GameObject[] checkPointGameObjects = GameObject.FindGameObjectsWithTag("CheckPoint");
		for (int i = 0; i < checkPointGameObjects.Length; i++) {
			checkPointGameObjects[i].GetComponent<CheckPoint>().Triggered = checkPoints[checkPointGameObjects[i].transform.position];
		}
	}

	public static void SaveCheckPoints() {
		GameObject[] checkPointGameObjects = GameObject.FindGameObjectsWithTag("CheckPoint");
		Debug.Log(checkPointGameObjects[0].GetComponent<CheckPoint>().Triggered);
		if (checkPoints == null) {
			checkPoints = new Dictionary<Vector3, bool>();
			for (int i = 0; i < checkPointGameObjects.Length; i++) {
				checkPoints.Add(checkPointGameObjects[i].transform.position, checkPointGameObjects[i].GetComponent<CheckPoint>().Triggered);
			}
		}
		else {
			for (int i = 0; i < checkPointGameObjects.Length; i++) {
				checkPoints[checkPointGameObjects[i].transform.position] = checkPointGameObjects[i].GetComponent<CheckPoint>().Triggered;
			}
		}
	}

	public static void ResetCheckPoints() {
		CaptureKeeper.ClearCaptures();
		checkPoints = null;
	}

}