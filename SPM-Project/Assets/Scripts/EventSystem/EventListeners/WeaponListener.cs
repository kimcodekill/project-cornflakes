using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg

public class WeaponListener : MonoBehaviour {

	private void Start() {
		//EventSystem.Current.RegisterListener<PickUpEvent>(OnPickUp);
		EventSystem.Current.RegisterListener<WeaponPickUpEvent>(OnWeaponPickUp);
		EventSystem.Current.RegisterListener<AmmoPickUpEvent>(OnAmmoPickUp);
	}

	private void OnPickUp(Event e) {
		PickUpEvent pue = (PickUpEvent) e;
		PlayerWeapon pw;
		if ((pw = pue.Target.GetComponent<PlayerWeapon>()) != null) {
			Weapon w;
			AmmoPickup ap;
			if ((w = pue.Source.GetComponent<Weapon>()) != null) {
				pw.PickUpWeapon(w);
				pue.Source.SetActive(false);
				pue.Target.GetComponentInChildren<PlayerHud>().ShowPickupText(w.ToString(), 0);
			}
			else if ((ap = pue.Source.GetComponent<AmmoPickup>()) != null) {
				pw.AddAmmo(ap.AmmoType, ap.AmmoAmount);
				pue.Target.GetComponent<PlayerController>().PlayAudioPitched(7, 1, 0.8f, 1.3f);
				pue.Source.SetActive(false);
				if (ap.AmmoAmount > 1 || ap.AmmoType == Weapon.EAmmoType.Special) pue.Target.GetComponentInChildren<PlayerHud>().ShowPickupText(ap.AmmoType.ToString().ToLower(), ap.AmmoAmount);
				else pue.Target.GetComponentInChildren<PlayerHud>().ShowPickupText(ap.AmmoType.ToString().Remove(ap.AmmoType.ToString().Length - 1).ToLower(), ap.AmmoAmount);
			}
		}
	}

	private void OnWeaponPickUp(Event e) {
		WeaponPickUpEvent wpue = e as WeaponPickUpEvent;

		PlayerWeapon.Instance.PickUpWeapon(wpue.Weapon);
		wpue.Source.SetActive(false);
		wpue.Target.GetComponentInChildren<PlayerHud>().ShowPickupText(wpue.Weapon.ToString(), 0);
	}

	private void OnAmmoPickUp(Event e) {
		AmmoPickUpEvent apue = e as AmmoPickUpEvent;

		PlayerWeapon.Instance.AddAmmo(apue.AmmoType, apue.AmmoAmount);
		
		//K: Not sure why we're telling the playercontroller to play audio but whatever
		apue.Target.GetComponent<PlayerController>().PlayAudioPitched(7, 1, 0.8f, 1.3f);
		apue.Source.SetActive(false);

		//K: This hud shit isnt very nice. We should redo it.
		if (apue.AmmoAmount > 1 || apue.AmmoType == Weapon.EAmmoType.Special) apue.Target.GetComponentInChildren<PlayerHud>().ShowPickupText(apue.AmmoType.ToString().ToLower(), apue.AmmoAmount);
		else apue.Target.GetComponentInChildren<PlayerHud>().ShowPickupText(apue.AmmoType.ToString().Remove(apue.AmmoType.ToString().Length - 1).ToLower(), apue.AmmoAmount);
	}
}