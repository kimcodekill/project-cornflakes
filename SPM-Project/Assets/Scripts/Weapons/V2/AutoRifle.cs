using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRifle : Weapon {

	public override void Fire() {
		Vector3 direction = AddSpread(GetDirectionToPoint(Muzzle.forward, GetCrosshairHitPoint()));
		//firegunevent
		if (Physics.Raycast(Muzzle.forward, direction, out RaycastHit hit, float.MaxValue, BulletHitMask)) {
			//hitsomethingevent
		}
		AmmoInMagazine--;
	}

}