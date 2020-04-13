using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon {

	#region Properties

	public int PelletCount { get => pelletCount; protected set => pelletCount = value; }

	#endregion

	#region Serialized
	
	[Header("Shotgun Attributes")]
	[SerializeField] private int pelletCount;

	#endregion

	public override void Fire() {
		FunctionPlayer.AddFunction(AddRecoil, 0.1f);
		AmmoInMagazine--;
	}

}