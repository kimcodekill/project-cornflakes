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
			public float score;
		}
		public PlayerStats Player;
		public List<Weapon> Weapons;
		public List<object> DichotomousGameObjects;
	}

	private static List<Capture> captures;

	/// <summary>
	/// If a scene load was triggered by a level transition rather than a death.
	/// </summary>
	public static bool NewLevel { get; set; }

	/// <summary>
	/// If this scene has been captured at least once
	/// </summary>
	public static bool LevelHasBeenCaptured { get; set; }

	#endregion

	#region Load Capture

	/// <summary>
	/// Loads the latest capture.
	/// If the current level is a new one we don't load weapons and dichotomous GameObjects.
	/// </summary>
	public static void LoadLatestCapture() {
		if (captures == null) return;
		Capture latestCapture = captures[captures.Count - 1];
		if (!NewLevel) {
			LoadWeaponCapture(latestCapture.Weapons);
			LoadPlayerCapture(latestCapture.Player);
		}
		if (LevelHasBeenCaptured) {
			LoadDichotomousGameObjectCapture(latestCapture.DichotomousGameObjects);
		}
		NewLevel = false;
	}

	/// <summary>
	///	Sets the player position, rotation and current weapon according to the last checkpoint.
	///	If a new scene is loaded, only the current weapon is set.
	/// </summary>
	private static void LoadPlayerCapture(Capture.PlayerStats player) {
		GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
		if (!NewLevel) {
			playerGameObject.transform.position = player.position;
			Camera.main.GetComponent<PlayerCamera>().InjectRotation(player.rotation.eulerAngles.x, player.rotation.eulerAngles.y);
		}
		playerGameObject.GetComponent<PlayerController>().PlayerCurrentHealth = player.health;
		PlayerHud.Instance.ScoreHandler.Score = player.score;
		if (player.currentWeapon != -1) PlayerWeapon.Instance.SwitchTo(player.currentWeapon);
	}

	/// <summary>
	/// Removes the players old weapons and replaces them with the ones equipped by the last checkpoint.
	/// </summary>
	private static void LoadWeaponCapture(List<Weapon> weapons) {
		PlayerWeapon.Instance.ResetInventory();

		for (int i = 0; i < weapons.Count; i++) {
			//Instantiate because we don't want the player to be able to modify captured weapons.
			Weapon w = Object.Instantiate(weapons[i]);
			w.Initialize(weapons[i].AmmoInMagazine, weapons[i].AmmoInReserve);
			PlayerWeapon.Instance.PickUpWeapon(w);
		}

		//K: no better place to do this
		if (PlayerWeapon.Instance.GetWeapons().Count == 0) { Reticle.Instance.Init(); }
	}

	/// <summary>
	/// Disables dichotomous GameObjects according to their state by the last checkpoint.
	/// Dichotomous - "dividing into two parts", a.k.a. we only care if these GameObjects are enabled or not,
	/// no other information about them is saved.
	/// </summary>
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
			Player = CapturePlayer(checkPoint == null ? PlayerController.Instance.transform.position : checkPoint.transform.position, checkPoint.transform.rotation),
			DichotomousGameObjects = CaptureDichotomousGameObjects()
		});
	}

	private static Capture.PlayerStats CapturePlayer(Vector3 checkPointPosition, Quaternion checkPointRotation) {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		return new Capture.PlayerStats() {
			position = checkPointPosition,
			rotation = checkPointRotation,
			health = player.GetComponent<PlayerController>().PlayerCurrentHealth,
			currentWeapon = PlayerWeapon.Instance.GetWeapons().IndexOf(PlayerWeapon.Instance.CurrentWeapon),
			score = PlayerHud.Instance.ScoreHandler.Score
		}; 
	}

	private static List<Weapon> CaptureWeapons() {
		List<Weapon> capturedWeapons = new List<Weapon>();
		List<Weapon> weapons = new List<Weapon>(PlayerWeapon.Instance.GetWeapons().ToArray());
		for (int i = 0; i < weapons.Count; i++) {
			Weapon w = Object.Instantiate(weapons[i]);
			w.Initialize(weapons[i].AmmoInMagazine, weapons[i].AmmoInReserve);
			capturedWeapons.Add(w);
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
		captures = null;
	}

	#endregion

	#region Helper Functions

	public static bool HasCapture() {
		return captures != null && captures.Count > 0;
	}

	#endregion

}