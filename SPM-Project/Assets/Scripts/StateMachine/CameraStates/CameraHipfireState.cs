using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CameraState/HipfireState")]
public class CameraHipfireState : CameraState
{
	//public int CameraFOV = 60;
	//public Vector3 CameraOffset = new Vector3(1.75f, 1.5f, -7f);

	public override void Run()
	{
		//No weapon? No hipfire.
		if (!PlayerWeapon.Instance.WeaponIsActive) { StateMachine.Pop(); }
		if (Input.GetKeyDown(KeyCode.Mouse1) && PlayerWeapon.Instance.CurrentWeapon.AmmoType == Weapon.EAmmoType.Special) { StateMachine.Push<CameraScopedState>(); }
		// ^: If the sniper is equipped and mouse1 is clicked, scope in.

		base.Run();
	}
}
