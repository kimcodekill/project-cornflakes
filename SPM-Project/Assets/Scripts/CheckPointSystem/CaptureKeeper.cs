using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
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
			public int currentWeapon;
		}
		public struct WeaponStats {
			public Weapon weapon;
			public int ammoInMagazine;
			public int ammoInReserve;
		}
		public PlayerStats Player;
		public List<WeaponStats> Weapons;
		public List<object> DichotomousGameObjects;
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
		LoadWeaponCapture(latestCapture.Weapons);
		LoadPlayerCapture(latestCapture.Player);
		LoadDichotomousGameObjectCapture(latestCapture.DichotomousGameObjects);
	}

	private static void LoadPlayerCapture(Capture.PlayerStats player) {
		GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
		playerGameObject.transform.position = player.position;
		Camera.main.GetComponent<PlayerCamera>().InjectRotation(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y);
		playerGameObject.GetComponent<PlayerController>().PlayerCurrentHealth = player.health;
		if (player.currentWeapon != -1) PlayerController.Instance.PlayerWeapon.SwitchTo(player.currentWeapon);
	}

	private static void LoadWeaponCapture(List<Capture.WeaponStats> weapons) {
		for (int i = 0; i < weapons.Count; i++) {
			Weapon weapon = weapons[i].weapon;
			weapon.enabled = true;
			weapon.Restart();
			weapon.AmmoInMagazine = weapons[i].ammoInMagazine;
			weapon.AmmoInReserve = weapons[i].ammoInReserve;
			PlayerController.Instance.PlayerWeapon.PickUpWeapon(weapon);
		}
	}

	private static void LoadDichotomousGameObjectCapture(List<object> capturedGameObjects) {
		GameObject[] gameObjects = Object.FindObjectsOfType<GameObject>();
		for (int i = 0; i < gameObjects.Length; i++) {
			ICapturable ic = gameObjects[i].GetComponent<ICapturable>();
			if (ic != null && ic.InstanceIsCapturable() && !capturedGameObjects.Contains(ic.GetPersistentCaptureID())) gameObjects[i].SetActive(false);
		}
	}

	#endregion

	#region Create Capture

	/// <summary>
	/// Creates a capture of the game state.
	/// </summary>
	/// <param name="checkPoint">The CheckPoint that triggered the capture.</param>
	public static void CreateCapture(CheckPoint checkPoint = null) {
		if (captures == null) captures = new List<Capture>();
		captures.Add(new Capture() {
			Weapons = CaptureWeapons(),
			Player = CapturePlayer(checkPoint == null ? PlayerController.Instance.transform.position : checkPoint.transform.position, Quaternion.identity),
			DichotomousGameObjects = CaptureDichotomousGameObjects()
		});
	}

	private static Capture.PlayerStats CapturePlayer(Vector3 checkPointPosition, Quaternion checkPointRotation) {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		return new Capture.PlayerStats() {
			position = checkPointPosition,
			rotation = checkPointRotation,
			health = player.GetComponent<PlayerController>().PlayerCurrentHealth,
			currentWeapon = PlayerController.Instance.PlayerWeapon.GetWeapons().IndexOf(PlayerController.Instance.PlayerWeapon.CurrentWeapon)
		}; 
	}

	private static List<Capture.WeaponStats> CaptureWeapons() {
		List<Capture.WeaponStats> capturedWeapons = new List<Capture.WeaponStats>();
		List<Weapon> weapons = new List<Weapon>(PlayerController.Instance.PlayerWeapon.GetWeapons().ToArray());
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

	private static List<object> CaptureDichotomousGameObjects() {
		GameObject[] gameObjects = Object.FindObjectsOfType<GameObject>();
		List<object> capturedGameObjects = new List<object>();
		for (int i = 0; i < gameObjects.Length; i++) {
			ICapturable ic = gameObjects[i].GetComponent<ICapturable>();
			if (ic != null && ic.InstanceIsCapturable()) {
				capturedGameObjects.Add(ic.GetPersistentCaptureID());
			}
		}
		return capturedGameObjects;
	}

	#endregion

	#region Manage Captures

	/// <summary>
	/// Removes the latest capture, causing the next scene load to start from the previous checkpoint.
	/// </summary>
	public static void RollBackCapture() {
		if (captures.Count == 1) captures = null;
		else captures.RemoveAt(captures.Count - 1);
	}

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