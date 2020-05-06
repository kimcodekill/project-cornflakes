using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootListener : MonoBehaviour {

	private void Start() => EventSystem.Current.RegisterListener<EnemyDeathEvent>(OnEnemyDeath);

	private void OnEnemyDeath(Event e) {
		EnemyDeathEvent ede = e as EnemyDeathEvent;
		ILootable il;
		if (Random.value <= ede.BaseDropChance && (il = ede.DeadObject.GetComponent<ILootable>()) != null) {
			string lootObjectPath;
			if ((lootObjectPath = il.GetLootTable().Roll()) != null) {
				GameObject go = Instantiate(Resources.Load<GameObject>(lootObjectPath), ede.DeadObject.transform.position, Quaternion.identity);
				go.AddComponent<BoxCollider>();
				Rigidbody rb = go.AddComponent<Rigidbody>();
				rb.angularDrag = 1f;
				rb.drag = 1f;
				rb.AddForceAtPosition(Vector3.up * 10 + Random.insideUnitSphere * 5, go.transform.position + Random.insideUnitSphere * 0.5f, ForceMode.Impulse);
				ISpawnable isp;
				if ((isp = go.GetComponent<ISpawnable>()) != null) isp.Spawned();
			}
		}
	}

}