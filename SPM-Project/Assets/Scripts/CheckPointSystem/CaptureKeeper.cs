using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class CaptureKeeper {

	#region Bookkeeping

	/// <summary>
	/// The container for all captured data.
	/// </summary>
	public struct Capture {
		public struct PlayerStats {
			public Vector3 position;
			public Quaternion rotation;
			public float health;
		}
		public struct WeaponStats {
			public Weapon weapon;
			public int ammoInMagazine;
			public int ammoInReserve;
		}
		public PlayerStats Player;
		public List<WeaponStats> Weapons;
		public List<Vector3> Enemies;
		public Dictionary<Vector3, bool> CheckPoints;
	}

	private static List<Capture> captures;

	#endregion

	#region Load Capture

	/// <summary>
	/// Loads the latest capture.
	/// </summary>
	public static void LoadLatestCapture() {
		if (captures == null) return;
		Capture latestCapture = captures[captures.Count - 1];
		LoadPlayerCapture(latestCapture.Player);
		LoadWeaponCapture(latestCapture.Weapons);
		LoadEnemyCapture(latestCapture.Enemies);
		LoadCheckPointCapture(latestCapture.CheckPoints);
	}

	private static void LoadPlayerCapture(Capture.PlayerStats player) {
		GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
		playerGameObject.transform.position = player.position;
		Camera.main.transform.rotation = player.rotation;
		playerGameObject.GetComponent<PlayerController>().PlayerCurrentHealth = player.health;
	}

	private static void LoadWeaponCapture(List<Capture.WeaponStats> weapons) {
		for (int i = 0; i < weapons.Count; i++) {
			Weapon weapon = weapons[i].weapon;
			weapon.enabled = true;
			weapon.AmmoInMagazine = weapons[i].ammoInMagazine;
			weapon.AmmoInReserve = weapons[i].ammoInReserve;
			PlayerWeapon.Instance.PickUpWeapon(weapon);
		}
	}

	private static void LoadEnemyCapture(List<Vector3> enemies) {
		GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
		for (int i = 0; i < enemyGameObjects.Length; i++) {
			if (!enemies.Contains(enemyGameObjects[i].transform.position)) enemyGameObjects[i].SetActive(false);
		}
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
			Weapons = CaptureWeapons(),
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

	private static List<Capture.WeaponStats> CaptureWeapons() {
		List<Capture.WeaponStats> capturedWeapons = new List<Capture.WeaponStats>();
		List<Weapon> weapons = new List<Weapon>(PlayerWeapon.Instance.GetWeapons().ToArray());
		for (int i = 0; i < weapons.Count; i++) {
			Weapon w = weapons[i];
			Object.DontDestroyOnLoad(w);
			capturedWeapons.Add(new Capture.WeaponStats() {
				weapon = w,
				ammoInMagazine = weapons[i].AmmoInMagazine,
				ammoInReserve = weapons[i].AmmoInReserve
			});
		}
		return capturedWeapons;
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

	#region Clear Captures

	/// <summary>
	/// Removes all captures.
	/// </summary>
	public static void ClearCaptures() {
		captures = null;
	}

	#endregion

}