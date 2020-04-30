using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class CaptureKeeper {

	public struct Capture {
		public struct Player {
			public Vector3 position;
			public Quaternion rotation;
			public float health;
		}
		public Player PlayerStats { get; private set; }
		public int[] Enemies { get; private set; }
		public Capture(int[] enemies, Player player) {
			Enemies = enemies;
			PlayerStats = player;
			Debug.Log("Captured " + enemies.Length + " enemies");
			Debug.Log("Captured player with " + player.health + " health");
		} 
	}

	private static List<Capture> captures;

	public static void LoadLatestCapture() {
		if (captures == null) return;
		Debug.Log("Loaded capture");
		Capture latestCapture = captures[captures.Count - 1];
		LoadPlayerCapture(latestCapture.PlayerStats);
		LoadEnemyCapture(latestCapture.Enemies);
	}

	private static void LoadEnemyCapture(int[] enemies) {
		GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Player");

	}

	private static void LoadPlayerCapture(Capture.Player player) {
		GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
		playerGameObject.transform.position = player.position;
		Camera.main.transform.rotation = player.rotation;
		playerGameObject.GetComponent<PlayerController>().PlayerCurrentHealth = player.health;
	}

	/// <summary>
	/// Creates a capture of the game state.
	/// </summary>
	/// <param name="checkPointPosition">The position the player should respawn at.</param>
	/// <param name="checkPointRotation">The rotation the player(or camera) should respawn facing.</param>
	public static void CreateCapture(Vector3 checkPointPosition, Quaternion checkPointRotation) {
		if (captures == null) captures = new List<Capture>();
		captures.Add(new Capture(CaptureEnemies(), CapturePlayer(checkPointPosition, checkPointRotation)));
	}

	private static Capture.Player CapturePlayer(Vector3 checkPointPosition, Quaternion checkPointRotation) {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		return new Capture.Player() {
			position = checkPointPosition,
			rotation = checkPointRotation,
			health = player.GetComponent<PlayerController>().PlayerCurrentHealth,
		}; 
	}

	private static int[] CaptureEnemies() {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		int[] hashCodes = new int[enemies.Length];
		for (int i = 0; i < enemies.Length; i++) {
			hashCodes[i] = enemies[i].GetHashCode();
		}
		return hashCodes;
	}

	public static void ClearCaptures() {
		captures = null;
	}


}