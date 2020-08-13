using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Joakim Linna

[CreateAssetMenu(menuName = "CameraState/DefaultState")]
public class CameraDefaultState : CameraState
{
	//public int CameraFOV = 60;
	//public Vector3 CameraOffset = new Vector3(0f, 1.5f, -7f);

	public override void Run()
	{
		// Kim: This will be broken if we deprecate WeaponIsActive (bc it's a deferred bool)

		// Weapon? Hipfire.
		if(PlayerWeapon.Instance.WeaponIsActive) { 
			StateMachine.Push<CameraHipfireState>(); 
		}

		base.Run();
	}
}
