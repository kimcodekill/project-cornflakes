using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class CaptureKeeper {

	public struct Capture {
		public struct PlayerStats {
			public Vector3 position;
			public Quaternion rotation;
			public float health;
		}
		public PlayerStats Player;
		public List<Vector3> Enemies;
		public Dictionary<Vector3, bool> CheckPoints;
	}

	private static List<Capture> captures;

	#region Load Capture

	public static void LoadLatestCapture() {
		if (captures == null) return;
		Capture latestCapture = captures[captures.Count - 1];
		LoadPlayerCapture(latestCapture.Player);
		LoadEnemyCapture(latestCapture.Enemies);
		LoadCheckPointCapture(latestCapture.CheckPoints);
	}

	private static void LoadEnemyCapture(List<Vector3> enemies) {
		GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
		for (int i = 0; i < enemyGameObjects.Length; i++) {
			if (!enemies.Contains(enemyGameObjects[i].transform.position)) enemyGameObjects[i].SetActive(false);
		}
	}

	private static void LoadPlayerCapture(Capture.PlayerStats player) {
		GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
		playerGameObject.transform.position = player.position;
		Camera.main.transform.rotation = player.rotation;
		playerGameObject.GetComponent<PlayerController>().PlayerCurrentHealth = player.health;
	}

	private static void LoadCheckPointCapture(Dictionary<Vector3, bool> checkPoints) {
		GameObject[] checkPointGameObjects = GameObject.FindGameObjectsWithTag("CheckPoint");
		for (int i = 0; i < checkPointGameObjects.Length; i++) {
			checkPointGameObjects[i].GetComponent<CheckPoint>().Triggered = checkPoints[checkPointGameObjects[i].transform.position];
		}
	}

	#endregion

	#region Create Capture

	/// <summary>
	/// Creates a capture of the game state.
	/// </summary>
	/// <param name="checkPointPosition">The position the player should respawn at.</param>
	/// <param name="checkPointRotation">The rotation the player(or camera) should respawn facing.</param>
	public static void CreateCapture(Vector3 checkPointPosition, Quaternion checkPointRotation) {
		if (captures == null) captures = new List<Capture>();
		captures.Add(new Capture() {
			Player = CapturePlayer(checkPointPosition, checkPointRotation),
			Enemies = CaptureEnemies(),
			CheckPoints = CaptureCheckPoints()
		});
	}

	private static Capture.PlayerStats CapturePlayer(Vector3 checkPointPosition, Quaternion checkPointRotation) {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		return new Capture.PlayerStats() {
			position = checkPointPosition,
			rotation = checkPointRotation,
			health = player.GetComponent<PlayerController>().PlayerCurrentHealth,
		}; 
	}

	private static List<Vector3> CaptureEnemies() {
		GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
		List<Vector3> enemies = new List<Vector3>();
		for (int i = 0; i < enemyGameObjects.Length; i++) {
			enemies.Add(enemyGameObjects[i].GetComponent<Enemy>().Origin);
		}
		return enemies;
	}

	private static Dictionary<Vector3, bool> CaptureCheckPoints() {
		GameObject[] checkPointGameObjects = GameObject.FindGameObjectsWithTag("CheckPoint");
		Dictionary<Vector3, bool> checkPoints = new Dictionary<Vector3, bool>();
		for (int i = 0; i < checkPointGameObjects.Length; i++) {
			checkPoints.Add(checkPointGameObjects[i].transform.position, checkPointGameObjects[i].GetComponent<CheckPoint>().Triggered);
		}
		return checkPoints;
	}

	#endregion

	public static void ClearCaptures() {
		captures = null;
	}


}