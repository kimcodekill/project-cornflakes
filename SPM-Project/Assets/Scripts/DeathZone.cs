﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour, IDamaging {
	
	public float GetDamage() {
		return 99999f;		
	}

	public float GetExplosionDamage(Vector3 explosionCenter, Vector3 hitPos)
	{
		throw new System.NotImplementedException();
	}

	private void OnTriggerEnter(Collider other) {
		EventSystem.Current.FireEvent(new HitEvent() {
			Source = gameObject,
			Target = other.gameObject,
			HitPoint = transform.position
		});
	}

}