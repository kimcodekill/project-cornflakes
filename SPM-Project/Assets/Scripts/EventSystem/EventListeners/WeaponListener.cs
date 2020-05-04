using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponListener : MonoBehaviour {

	private void Start()
	{
		EventSystem.Current.RegisterListener<PickUpEvent>(OnPickUp);
		EventSystem.Current.RegisterListener<WeaponFiredEvent>(OnWeaponFired);
	}

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

	private void OnWeaponFired(Event e)
	{
		WeaponFiredEvent wfe = e as WeaponFiredEvent;

		//Trigger the weapon recoil animation here
		//If you need something special about the event, wfe contains some stuff

	}
}