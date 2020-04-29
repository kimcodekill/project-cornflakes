﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CameraState/ScopedState")]
public class CameraScopedState : CameraState {

	/// <summary>
	/// Whether or not Mouse1 needs to be held down to keep aiming, or if aim is toggled with a click.
	/// </summary>
	public bool Toggle;
	
	public override void Run() {
		//In order of appearance: Stop aiming if weapon is no longer active, or the equipped weapon isn't a sniper rifle, or toggle mode is used and the aim key was pressed, or toggle mode isn't used and the aim key simply was released.
		if (!PlayerWeapon.Instance.WeaponIsActive || !(PlayerWeapon.Instance.CurrentWeapon is SniperRifle) || (Toggle && Input.GetKeyDown(KeyCode.Mouse1)) || (!Toggle && Input.GetKeyUp(KeyCode.Mouse1))) {
			StateMachine.TransitionTo<CameraHipfireState>();
		}

		base.Run();
	}

	public override bool CanEnter() {
		return PlayerWeapon.Instance.CurrentWeapon != null && PlayerWeapon.Instance.CurrentWeapon is SniperRifle;
	}

}