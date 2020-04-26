using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CameraState/ScopedState")]
public class CameraScopedState : CameraState
{
	//public int CameraFOV = 30;
	//public Vector3 CameraOffset = new Vector3(1.75f, 1.5f, -7f);

	public override void Run()
	{
		//	If the player unequipped weapon, equipped a different weapon, or pressed mouse1 :
		//		Go back to Hipfire;
		//  (if the player unequipped, then HipfireState will also pop)
		if ((!PlayerWeapon.Instance.WeaponIsActive || PlayerWeapon.Instance.CurrentWeapon.AmmoType != Weapon.EAmmoType.Special) || Input.GetKeyDown(KeyCode.Mouse1)) { StateMachine.Pop(); }

		base.Run();
	}
}
