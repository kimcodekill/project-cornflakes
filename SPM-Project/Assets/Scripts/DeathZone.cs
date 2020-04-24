using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour, IDamaging {
	
	public float GetDamage() {
		return 99999f;		
	}

	private void OnTriggerEnter(Collider other) {
		EventSystem.Current.FireEvent(new HitEvent() {
			Source = gameObject,
			Target = other.gameObject,
			HitPoint = transform.position
		});
	}

}