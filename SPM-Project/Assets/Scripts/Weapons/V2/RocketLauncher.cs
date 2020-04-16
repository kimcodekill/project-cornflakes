using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Weapon {

	#region Properties

	/// <summary>
	/// The speed of the launched projectile.
	/// </summary>
	public float ProjectileSpeed { get => projectileSpeed; protected set => projectileSpeed = value; }

	/// <summary>
	/// The area of effect of the launched projectile, once it hits something.
	/// </summary>
	public float ProjectileAreaOfEffect { get => projectileAreaOfEffect; protected set => projectileAreaOfEffect = value; }

	#endregion

	#region Serialized

	[SerializeField] private float projectileSpeed;
	[SerializeField] private float projectileAreaOfEffect;

	#endregion

	public override void Fire() {
		Vector3 direction = AddSpread(GetDirectionToPoint(Muzzle.forward, GetCrosshairHit().point));
		//firegunevent
		if (Physics.Raycast(Muzzle.forward, direction, out RaycastHit hit, float.MaxValue, BulletHitMask)) {
			//hitsomethingevent
		}
		AmmoInMagazine--;
	}

}