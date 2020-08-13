﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class AmmoPickup : Pickup {

	#region Properties

	public Weapon.EAmmoType AmmoType { get => ammoType; }

	public int AmmoAmount { get => ammoAmount; }

	#endregion

	#region Serialized

	[SerializeField] private Weapon.EAmmoType ammoType;
	[SerializeField] private int ammoAmount;
	[SerializeField] private int spawnedAmmoAmount;

	#endregion

	protected override void OnPickup(Collider other) {

		EventSystem.Current.FireEvent(new AmmoPickUpEvent(gameObject, other.gameObject, ammoType, ammoAmount));
	}

	protected override bool IsValid(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			PlayerWeapon.AmmoPool ap = PlayerWeapon.Instance.GetAmmoPool(ammoType);
			if (ap.reserveAmmo >= ap.maxAmmo) {
				other.GetComponentInChildren<PlayerHud>().ShowPickupText(ammoType.ToString(), 0, "full");
				return false;
			}
			if (ap.reserveAmmo + ammoAmount > ap.maxAmmo) ammoAmount = ap.maxAmmo - ap.reserveAmmo;
			return true;
		}
		else return false;
	}

	protected override void OnSpawned() {
		ammoAmount = spawnedAmmoAmount;
	}

}