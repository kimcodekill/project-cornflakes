using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootListener : MonoBehaviour {

	private void Start() => EventSystem.Current.RegisterListener<EnemyDeathEvent>(OnEnemyDeath);

	private void OnEnemyDeath(Event e) {
		EnemyDeathEvent ede = e as EnemyDeathEvent;
		ILootable il;
		if (Random.value <= ede.DropAnythingAtAllChance && (il = ede.Source.GetComponent<ILootable>()) != null) {
			string lootObjectPath;
			if ((lootObjectPath = il.GetLootTable().Roll()) != null) {
				GameObject go = Instantiate(Resources.Load<GameObject>(lootObjectPath), ede.Source.transform.position, Quaternion.identity);
				ISpawnable isp;
				if ((isp = go.GetComponent<ISpawnable>()) != null) isp.Spawned();
			}
		}
	}

}