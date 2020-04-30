using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathListener : MonoBehaviour
{
	private void Start() => EventSystem.Current.RegisterListener<EnemyDeathEvent>(OnDeath);

	private void OnDeath(Event e) {
		EnemyDeathEvent ede = (EnemyDeathEvent) e;
		Debug.Log(ede.Description);
	}
}
