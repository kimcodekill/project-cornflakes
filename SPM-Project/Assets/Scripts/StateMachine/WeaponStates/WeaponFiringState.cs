using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponState/FiringState")]
public class WeaponFiringState : WeaponState {

	public override void Enter() {
		DebugManager.UpdateRow("WeaponSTM" + Weapon.gameObject.GetInstanceID(), GetType().ToString());
	}

	public override void Run() {
		Weapon.Fire();
		if (!Weapon.HasAmmoInMagazine()) {
			if (Weapon.HasAmmoInReserve()) StateMachine.TransitionTo<WeaponReloadingState>();
			else StateMachine.TransitionTo<WeaponIdleState>();
		}
		else if (!Weapon.FullAuto) StateMachine.TransitionTo<WeaponIdleState>();

		base.Run();
	}

}