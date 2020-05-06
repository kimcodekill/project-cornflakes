using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class WeaponPickup : Pickup {

	protected override void OnPickup(Collider other) {
		EventSystem.Current.FireEvent(new PickUpEvent() {
			Description = this + " was picked up by " + other.gameObject,
			Source = gameObject,
			Target = other.gameObject,
		});
	}

	protected override bool IsValid(Collider other) {
		return other.gameObject.CompareTag("Player");
	}

}