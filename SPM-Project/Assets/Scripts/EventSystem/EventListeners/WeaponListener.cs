using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg

public class WeaponListener : MonoBehaviour {

	private void Start() {
		//EventSystem.Current.RegisterListener<PickUpEvent>(OnPickUp);
		EventSystem.Current.RegisterListener<WeaponPickUpEvent>(OnWeaponPickUp);
		EventSystem.Current.RegisterListener<AmmoPickUpEvent>(OnAmmoPickUp);
		EventSystem.Current.RegisterListener<WeaponFiredEvent>(OnWeaponFired);
	}

	private void OnWeaponFired(Event e) {
		PlayerController.Instance.playerAnimator.SetTrigger("Shooting");
	}

	private void OnWeaponPickUp(Event e) {
		WeaponPickUpEvent wpue = e as WeaponPickUpEvent;

		PlayerWeapon.Instance.PickUpWeapon(wpue.Weapon);
		wpue.Other.GetComponentInChildren<PlayerHud>().ShowPickupText(wpue.Weapon.ToString(), 0, "picked up");
		wpue.Other.GetComponentInChildren<PlayerHud>().NewCarriedWeapon(PlayerWeapon.Instance.GetWeapons().Count - 1);
		wpue.Pickup.SetActive(false);
	}

	private void OnAmmoPickUp(Event e) {
		AmmoPickUpEvent apue = e as AmmoPickUpEvent;

		PlayerWeapon.Instance.AddAmmo(apue.AmmoType, apue.AmmoAmount);
		
		//K: Not sure why we're telling the playercontroller to play audio but whatever
		apue.Other.GetComponent<PlayerController>().PlayAudioPitched(7, 1, 0.8f, 1.3f);
		apue.Pickup.SetActive(false);

		//K: This hud shit isnt very nice. We should redo it.
		if (apue.AmmoAmount > 1 || apue.AmmoType == Weapon.EAmmoType.Special) apue.Other.GetComponentInChildren<PlayerHud>().ShowPickupText(apue.AmmoType.ToString().ToLower(), apue.AmmoAmount, "picked up");
		else apue.Other.GetComponentInChildren<PlayerHud>().ShowPickupText(apue.AmmoType.ToString().Remove(apue.AmmoType.ToString().Length - 1).ToLower(), apue.AmmoAmount, "picked up");
	}
}