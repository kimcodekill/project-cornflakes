using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponState : State {

	private Weapon weapon;

	public Weapon Weapon => weapon = weapon != null ? weapon : (Weapon) Owner;

	public override void Run() {
		if (Weapon.RequestedReload && Weapon.AmmoInMagazine < Weapon.MagazineSize && Weapon.HasAmmoInReserve()) StateMachine.TransitionTo<WeaponReloadingState>();
	}

}