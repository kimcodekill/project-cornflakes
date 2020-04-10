using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponState/FiringState")]
public class WeaponFiringState : WeaponState {

	private float delay;

	public override void Enter() {
		DebugManager.UpdateRow("WeaponSTM" + Weapon.gameObject.GetInstanceID(), GetType().ToString());
		delay = Weapon.GetTimeBetweenShots();
	}

	public override void Run() {
		if ((Weapon.FullAuto && (delay += Time.deltaTime) > Weapon.GetTimeBetweenShots()) || !Weapon.FullAuto) {
			Weapon.Fire();
			delay = 0f;
		}
		if (!Weapon.HasAmmoInMagazine()) {
			if (Weapon.HasAmmoInReserve()) StateMachine.TransitionTo<WeaponReloadingState>();
			else StateMachine.TransitionTo<WeaponIdleState>();
		}
		else if (!Weapon.FullAuto || !Weapon.TriggerDown) StateMachine.TransitionTo<WeaponIdleState>();

		base.Run();
	}

}