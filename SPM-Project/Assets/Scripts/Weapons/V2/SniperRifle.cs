using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : Weapon {

	#region Properties

	/// <summary>
	/// Whether or not the weapon is being aimed down sights with.
	/// </summary>
	public bool AimingDownSights { get { return Input.GetKey(KeyCode.Mouse1); } }

	/// <summary>
	/// The amount the camera should zoom in if the weapon is being aimed down sights with.
	/// </summary>
	public float ZoomFactor { get => zoomFactor; protected set => zoomFactor = value; }

	#endregion

	#region Serialized

	[Header("Sniper Rifle Attributes")]
	[SerializeField] private float zoomFactor;

	#endregion

	public override void Fire() {
		Vector3 direction = AddSpread(GetDirectionToPoint(Muzzle.forward, GetCrosshairHitPoint()));
		//firegunevent
		if (Physics.Raycast(Muzzle.forward, direction, out RaycastHit hit, float.MaxValue, BulletHitMask)) {
			//hitsomethingevent
		}
		AmmoInMagazine--;
	}

}