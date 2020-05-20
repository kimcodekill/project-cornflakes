﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class DeathZone : MonoBehaviour, IDamaging {
	
	public float GetDamage() {
		return 99999f;		
	}

	public float GetExplosionDamage(Vector3 explosionCenter, Vector3 hitPos)
	{
		throw new System.NotImplementedException();
	}

	private void OnTriggerEnter(Collider other) {
		IEntity entity;
		
		if((entity = other.gameObject.GetComponent<IEntity>()) != null)
		{
			
		}
	}

}