using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : Weapon {

	public override void Fire() {
		AmmoInMagazine--;
	}

}