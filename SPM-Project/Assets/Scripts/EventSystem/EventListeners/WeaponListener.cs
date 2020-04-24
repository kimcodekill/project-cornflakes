using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponListener : MonoBehaviour {

	private void Start() => EventSystem.Current.RegisterListener<PickUpEvent>(OnPickUp);

	private void OnPickUp(Event e) {
		PickUpEvent pue = (PickUpEvent) e;
		Debug.Log(pue.Description);
		PlayerWeapon pw;
		if ((pw = pue.Target.GetComponent<PlayerWeapon>()) != null) {
			Weapon w;
			AmmoPickup ap;
			if ((w = pue.Source.GetComponent<Weapon>()) != null) {
				pw.PickUpWeapon(w);
				pue.Source.SetActive(false);
			}
			else if ((ap = pue.Source.GetComponent<AmmoPickup>()) != null) {
				pw.AddAmmo(ap.AmmoType, ap.AmmoAmount);
				pue.Source.SetActive(false);
			}
		}
	}

}