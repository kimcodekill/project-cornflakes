using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponState/FiringState")]
public class WeaponFiringState : WeaponState {

	private float delay;

	private bool waitForSemi;

	public override void Enter() {
		//DebugManager.UpdateRow("WeaponSTM", GetType().ToString());
		
		delay = Weapon.GetTimeBetweenShots();
		waitForSemi = false;
	}

	public override void Run() {
		delay += Time.deltaTime;
		if (!waitForSemi && ((Weapon.FullAuto && delay > Weapon.GetTimeBetweenShots()) || !Weapon.FullAuto)) {
			Weapon.DoFire();
			delay = 0f;
			if (!Weapon.FullAuto) waitForSemi = true;
		}
		if (delay > Weapon.GetTimeBetweenShots()) waitForSemi = false;
		if (!Weapon.HasAmmoInMagazine()) {
			if (Weapon.HasAmmoInReserve()) StateMachine.TransitionTo<WeaponReloadingState>();
			else StateMachine.TransitionTo<WeaponIdleState>();
		}
		else if (!Weapon.FullAuto || !Weapon.TriggerDown) {
			if (!waitForSemi) StateMachine.TransitionTo<WeaponIdleState>();
		}

		base.Run();
	}

}