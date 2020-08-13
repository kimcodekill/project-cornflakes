using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
//Co Author: Joakim Linna

public abstract class WeaponState : State {

	private PlayerWeapon weaponOwner;

	public Weapon Weapon => weaponOwner == null ? (weaponOwner = (PlayerWeapon)Owner).CurrentWeapon : weaponOwner.CurrentWeapon;

	public override void Run() {
		//if (Weapon.RequestedReload && Weapon.AmmoInMagazine < Weapon.MagazineSize && Weapon.HasAmmoInReserve()) StateMachine.TransitionTo<WeaponReloadingState>();

		if(weaponOwner.SwitchWeapon) {
			StateMachine.TransitionTo<WeaponIdleState>(); 
		}
	}

}