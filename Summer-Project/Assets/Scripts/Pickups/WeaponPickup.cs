﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
//Co Author: Joakim Linna
public class WeaponPickup : Pickup {

	[SerializeField] private Weapon weapon;

	protected override void Init()
	{
		weapon.Initialize();
	}

	protected override void OnPickup(Collider other) {
		
		EventSystem.Current.FireEvent(new WeaponPickUpEvent(gameObject, other.gameObject, weapon));
	}

	protected override bool IsValid(Collider other) {
		return other.gameObject.CompareTag("Player");
	}
}