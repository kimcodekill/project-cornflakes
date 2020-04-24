using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour {

	#region Properties

	public Weapon.EAmmoType AmmoType { get => ammoType; }

	public int AmmoAmount { get => ammoAmount; }

	#endregion

	#region Serialized

	[SerializeField] private Weapon.EAmmoType ammoType;
	[SerializeField] private int ammoAmount;

	#endregion

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			EventSystem.Current.FireEvent(new PickUpEvent() {
				Description = this + " was picked up by " + other.gameObject,
				Source = gameObject,
				Target = other.gameObject,
			});
		}
	}

}