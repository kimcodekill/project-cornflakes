using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : Pickup {

	#region Properties

	public Weapon.EAmmoType AmmoType { get => ammoType; }

	public int AmmoAmount { get => ammoAmount; }

	#endregion

	#region Serialized

	[SerializeField] private Weapon.EAmmoType ammoType;
	[SerializeField] private int ammoAmount;

	#endregion

	protected override void OnPickup(Collider other) {
		EventSystem.Current.FireEvent(new PickUpEvent() {
			Description = this + " was picked up by " + other.gameObject,
			Source = gameObject,
			Target = other.gameObject,
		});
	}

	protected override bool IsValid(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			PlayerWeapon.AmmoPool ap = PlayerWeapon.Instance.GetAmmoPool(ammoType);
			if (ap.reserveAmmo >= ap.maxAmmo) return false;
			if (ap.reserveAmmo + ammoAmount > ap.maxAmmo) ammoAmount = ap.maxAmmo - ap.reserveAmmo;
			return true;
		}
		else return false;
	}

}