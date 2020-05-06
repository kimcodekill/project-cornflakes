using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Erik Pilström
public class EnemyDeathListener : MonoBehaviour
{
	private void Start() => EventSystem.Current.RegisterListener<EnemyDeathEvent>(OnDeath);

	private void OnDeath(Event e) {
		EnemyDeathEvent ede = (EnemyDeathEvent) e;
		Debug.Log(ede.Description);
	}
}
