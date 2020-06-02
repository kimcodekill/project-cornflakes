using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class DeathZone : MonoBehaviour, IDamaging {
	
	public float GetDamage() {
		return 99999f;		
	}

	private void OnTriggerEnter(Collider other) {
		IEntity entity;
		
		if((entity = other.gameObject.GetComponent<IEntity>()) != null)
		{
			EventSystem.Current.FireEvent(new DamageEvent(entity, this));
		}
	}

}