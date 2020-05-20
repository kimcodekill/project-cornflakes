using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class WeaponPickup : Pickup {

	Weapon weapon;

	private void Start()
	{
		weapon = GetComponent<Weapon>();
	}

	protected override void OnPickup(Collider other) {
		EventSystem.Current.FireEvent(new PickUpEvent() {
			Description = this + " was picked up by " + other.gameObject,
			Source = gameObject,
			Target = other.gameObject,
		});

		//Kim: i added a new kind of pickupevent called WeaponPickUpEvent
		//     that lets us send the weapon immediately.
		EventSystem.Current.FireEvent(new WeaponPickUpEvent()
		{
			Description = weapon + " was picked up by " + other.gameObject,
			Source = gameObject,
			Target = other.gameObject
		}) ;
	}

	protected override bool IsValid(Collider other) {
		return other.gameObject.CompareTag("Player");
	}

}