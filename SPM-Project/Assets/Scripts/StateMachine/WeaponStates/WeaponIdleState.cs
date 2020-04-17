using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponState/IdleState")]
public class WeaponIdleState : WeaponState {

	private bool ignoreTriggerDown;

	private bool ignoredTriggerDown;

	public override void Enter() {
		try { DebugManager.AddSection("WeaponSTM", "", "", "", ""); } catch (System.ArgumentException) { }
		DebugManager.UpdateRow("WeaponSTM", GetType().ToString());

		if (!Weapon.FullAuto && Weapon.TriggerDown) {
			ignoreTriggerDown = true;
			ignoredTriggerDown = false;
		}
		else ignoredTriggerDown = true;
	}

	public override void Run() {
		if (!Weapon.TriggerDown && !ignoredTriggerDown) {
			ignoreTriggerDown = false;
			ignoredTriggerDown = true;
		}
		if (Weapon.TriggerDown && ignoredTriggerDown) {
			if (Weapon.HasAmmoInMagazine()) StateMachine.TransitionTo<WeaponFiringState>();
			else if (Weapon.HasAmmoInReserve()) StateMachine.TransitionTo<WeaponReloadingState>();
		}
		//"Cool down" auto rifle if idle
		if (Weapon is AutoRifle) Weapon.AmmoInMagazine = Weapon.AmmoInMagazine + 1 > Weapon.MagazineSize ? Weapon.MagazineSize : Weapon.AmmoInMagazine + 1;
		base.Run();
	}

}