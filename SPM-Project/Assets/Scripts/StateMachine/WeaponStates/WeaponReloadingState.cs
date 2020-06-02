using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
//Co Author: Joakim Linna

[CreateAssetMenu(menuName = "WeaponState/ReloadingState")]
public class WeaponReloadingState : WeaponState {

	private float startTime;

	public override void Enter() {
		DebugManager.UpdateRow("WeaponSTM", GetType().ToString());
		EventSystem.Current.FireEvent(new WeaponReloadingEvent(Weapon.ReloadAudio, PlayerWeapon.Instance.WeaponAudio, true));
		startTime = Time.time;
	}

	public override void Run() {
		if ((Time.time - startTime) > Weapon.ReloadTime) {
			Weapon.Reload();
			StateMachine.TransitionTo<WeaponIdleState>();
		}

		base.Run();
	}
}