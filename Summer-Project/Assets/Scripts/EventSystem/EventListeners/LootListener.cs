﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootListener : MonoBehaviour {

	private void Start() => EventSystem.Current.RegisterListener<EnemyDeathEvent>(OnEnemyDeath);

	private void OnEnemyDeath(Event e) {
		EnemyDeathEvent ede = e as EnemyDeathEvent;
		SpawnItem("Pickups/HealthPickup", ede.DeadObject.transform.position);
		ILootable il;
		if ((il = ede.DeadObject.GetComponent<ILootable>()) != null) {
			string lootObjectPath;
			if ((lootObjectPath = il.GetLootTable().Roll()) != null) {
				SpawnItem(lootObjectPath, ede.DeadObject.transform.position);
			}
		}
	}

	private void SpawnItem(string path, Vector3 position) {
		GameObject go = Instantiate(Resources.Load<GameObject>(path), position, Quaternion.identity);
		go.AddComponent<BoxCollider>();
		Rigidbody rb = go.AddComponent<Rigidbody>();
		rb.angularDrag = 1f;
		rb.drag = 1f;
		rb.AddForceAtPosition(Vector3.up * 10 + Random.insideUnitSphere * 5, go.transform.position + Random.insideUnitSphere * 0.5f, ForceMode.Impulse);
		ISpawnable isp;
		if ((isp = go.GetComponent<ISpawnable>()) != null) isp.Spawned();
	}

}