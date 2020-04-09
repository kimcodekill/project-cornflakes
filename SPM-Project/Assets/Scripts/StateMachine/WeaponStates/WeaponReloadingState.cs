using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WeaponState/ReloadingState")]
public class WeaponReloadingState : WeaponState {

	public float ReloadTime = 2f;

	private float currentTime;

	public override void Enter() {
		DebugManager.UpdateRow("WeaponSTM" + Weapon.gameObject.GetInstanceID(), GetType().ToString());

		currentTime = 0f;
	}

	public override void Run() {
		if ((currentTime += Time.deltaTime) > ReloadTime) {
			Weapon.Reload();
			StateMachine.TransitionTo<WeaponIdleState>();
		}
	}

}