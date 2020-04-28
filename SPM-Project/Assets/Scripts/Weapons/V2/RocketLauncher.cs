using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The rocket launcher fires relatively slow moving rockets instead of bullets.
/// </summary>
public class RocketLauncher : Weapon {

	#region Properties

	/// <summary>
	/// The speed of the launched projectile.
	/// </summary>
	public float RocketSpeed { get => rocketSpeed; protected set => rocketSpeed = value; }

	/// <summary>
	/// The area of effect of the launched projectile, once it hits something.
	/// </summary>
	public float RocketAreaOfEffect { get => rocketAreaOfEffect; protected set => rocketAreaOfEffect = value; }

	#endregion

	#region Serialized

	[Header("Rocket Launcher Properties")]
	[SerializeField] private float rocketSpeed;
	[SerializeField] private float rocketAreaOfEffect;
	[SerializeField] private float rocketLifeTime;
	[SerializeField] private GameObject rocket;

	#endregion

	protected override void Fire() {
		Vector3 direction = GetDirectionToPoint(Muzzle.position, GetCrosshairHitPoint());
		RocketV2 launchedRocket = Instantiate(rocket, Muzzle.position, Quaternion.LookRotation(direction, Vector3.up)).GetComponent<RocketV2>();
		launchedRocket.Damage = Damage;
		launchedRocket.Speed = rocketSpeed;
		launchedRocket.AreaOfEffect = rocketAreaOfEffect;
		launchedRocket.TargetDir = (MuzzleCast().point - Muzzle.position).normalized;
		launchedRocket.LifeTime = rocketLifeTime;
		AmmoInMagazine--;
	}

}