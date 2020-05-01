using System.Collections;
using System.Collections.Generic;
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
		public List<object> Enemies;
		public List<object> CheckPoints;
		public List<object> Pickups;
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
		LoadGameObjectCapture(latestCapture.CheckPoints, "CheckPoint");
		LoadGameObjectCapture(latestCapture.Pickups, "Pickup");
	}

	private static void LoadPlayerCapture(Capture.PlayerStats player) {
		GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
		playerGameObject.transform.position = player.position;
		Camera.main.GetComponent<PlayerCamera>().InjectRotation(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y);
		playerGameObject.GetComponent<PlayerController>().PlayerCurrentHealth = player.health;
	}

	private static void LoadWeaponCapture(List<Capture.WeaponStats> weapons) {
		for (int i = 0; i < weapons.Count; i++) {
			Weapon weapon = weapons[i].weapon;
			weapon.enabled = true;
			weapon.Restart();
			weapon.AmmoInMagazine = weapons[i].ammoInMagazine;
			weapon.AmmoInReserve = weapons[i].ammoInReserve;
			PlayerWeapon.Instance.PickUpWeapon(weapon);
		}
	}

	private static void LoadEnemyCapture(List<object> enemies) {
		GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
		for (int i = 0; i < enemyGameObjects.Length; i++) {
			if (!enemies.Contains(enemyGameObjects[i].transform.position)) enemyGameObjects[i].SetActive(false);
		}
	}

	private static void LoadGameObjectCapture(List<object> capturedGameObjects, string tag) {
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
		for (int i = 0; i < gameObjects.Length; i++) {
			if (!capturedGameObjects.Contains(gameObjects[i].transform.position)) gameObjects[i].SetActive(false);
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
			Enemies = CaptureGameObjects("Enemy"),
			CheckPoints = CaptureGameObjects("CheckPoint"),
			Pickups = CaptureGameObjects("Pickup")
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

	private static List<object> CaptureGameObjects(string tag) {
		//GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
		GameObject[] gameObjects = Object.FindObjectsOfType<GameObject>();
		List<object> capturedGameObjects = new List<object>();
		for (int i = 0; i < gameObjects.Length; i++) {
			ICapturable ic = gameObjects[i].GetComponent<ICapturable>();
			if (ic != null && ic.InstanceIsCapturable()) {
				capturedGameObjects.Add(gameObjects[i].GetComponent<ICapturable>().GetPersistentCaptureID());
			}
		}
		return capturedGameObjects;
	}

	#endregion

	#region Clear Captures

	/// <summary>
	/// Removes all captures.
	/// </summary>
	public static void ClearCaptures() {
		DestroyDontDestroyOnLoad();
		captures = null;
	}

	#endregion

	#region Helper Functions

	private static void DestroyDontDestroyOnLoad() {
		for (int i = 0; i < captures.Count; i++) {
			for (int j = 0; j < captures.Count; j++) {
				Object.Destroy(captures[i].Weapons[j].weapon.gameObject);
			}
		}
	}

	#endregion

}