using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Joakim Linna

[CreateAssetMenu(menuName = "CameraState/HipfireState")]
public class CameraHipfireState : CameraState {

	public override void Run() {
		if (PlayerController.Instance.PlayerWeapon.WeaponIsActive && Input.GetKeyDown(KeyCode.Mouse1) && StateMachine.CanEnterState<CameraScopedState>()) { StateMachine.TransitionTo<CameraScopedState>(); }

		base.Run();
	}

}