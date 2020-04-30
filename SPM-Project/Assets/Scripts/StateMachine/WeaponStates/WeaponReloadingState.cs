using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponState/ReloadingState")]
public class WeaponReloadingState : WeaponState {

	private float currentTime;

	public override void Enter() {
		//DebugManager.UpdateRow("WeaponSTM", GetType().ToString());

		currentTime = 0f;
	}

	public override void Run() {
		if ((currentTime += Time.deltaTime) > Weapon.ReloadTime) {
			Weapon.Reload();
			StateMachine.TransitionTo<WeaponIdleState>();
		}
	}

}