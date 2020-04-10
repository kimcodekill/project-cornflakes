using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon {

	public override void Fire() {
		AmmoInMagazine--;
	}

}