using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard : MonoBehaviour{

	/// <summary>
	/// Singleton
	/// </summary>
	public static BlackBoard Instance;

	private struct GameCapture {

		private struct WeaponCapture {
			Weapon w;
			int ammoInMagazine;
			int ammoInReserve;
		}

		private struct EnemyCapture {
			int health;
		}

		private List<WeaponCapture> weapons;

		private List<EnemyCapture> enemies;

		//private Dictionary<Pickup, bool> pickups;
	}

	private static bool newSession = true;
	private static bool newLevel = true;

	private static List<GameObject> enemies; 
	
	private static List<GameObject> pickups;

	private void OnEnable() {
		if (Instance == null) Instance = this;
	}

	private void Start() {
		if (newSession || newLevel) {
			EstablishGameObjectLists();
			newSession = false;
			newLevel = false;
		}
	}

	private void EstablishGameObjectLists() {
		GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
		for (int i = 0; i < allGameObjects.Length; i++) {
			if (allGameObjects[i].gameObject.GetComponent<Enemy>()) enemies.Add(allGameObjects[i]);
		}
	}

	public void CreateCapture() {
		foreach (GameObject e in enemies) {

		}
	}


}