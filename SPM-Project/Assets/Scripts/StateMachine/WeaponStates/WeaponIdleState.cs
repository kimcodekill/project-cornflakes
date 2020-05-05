using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
//Co Author: Joakim Linna

[CreateAssetMenu(menuName = "WeaponState/IdleState")]
public class WeaponIdleState : WeaponState {

	public override void Enter() {
		DebugManager.UpdateRow("WeaponSTM", GetType().ToString());
	}

	public override void Run() {

		if (Weapon.HasAmmoInMagazine())
		{
			if (Weapon.TriggerPulled)
			{
				StateMachine.TransitionTo<WeaponFiringState>();
			}
		} 
		else
		{
			if (Weapon.HasAmmoInReserve())
			{
				StateMachine.TransitionTo<WeaponReloadingState>();
			}
		}

		//"Cool down" auto rifle if idle
		if (Weapon is AutoRifle)
		{
			if (((Weapon as AutoRifle).CurrentCooldownTime += Time.deltaTime) > (Weapon as AutoRifle).CooldownWait)
			{
				Weapon.AmmoInMagazine += Weapon.AmmoInMagazine < Weapon.MagazineSize ? 1 : 0;
			}
		}
		else
		{
			if (Weapon.AmmoInMagazine < Weapon.MagazineSize && Weapon.HasAmmoInReserve() && Weapon.RequestedReload)
			{
				StateMachine.TransitionTo<WeaponReloadingState>();
			}
		}

		base.Run();
	}
}