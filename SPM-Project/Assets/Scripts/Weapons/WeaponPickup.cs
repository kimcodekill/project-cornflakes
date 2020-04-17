using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour {

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