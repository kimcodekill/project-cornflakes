using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The rocket launcher fires relatively slow moving rockets instead of bullets.
/// </summary>
public class RocketLauncher : Weapon {

    #region PreWeaponRework

    //#region Properties

    ///// <summary>
    ///// The speed of the launched projectile.
    ///// </summary>
    //public float RocketSpeed { get => rocketSpeed; protected set => rocketSpeed = value; }

    ///// <summary>
    ///// The area of effect of the launched projectile, once it hits something.
    ///// </summary>
    //public float RocketAreaOfEffect { get => rocketAreaOfEffect; protected set => rocketAreaOfEffect = value; }

    //#endregion

    //#region Serialized

    //[Header("Rocket Launcher Properties")]
    //[SerializeField] private float rocketSpeed;
    //[SerializeField] private float rocketAreaOfEffect;
    //[SerializeField] private float rocketLifeTime;

    #endregion

    //This is now the only value that is required.
    [SerializeField] private GameObject rocket;

	protected override void Fire() {
		Vector3 direction = GetDirectionToPoint(Muzzle.position, GetCrosshairHitPoint());
		RocketV2 launchedRocket = Instantiate(rocket, Muzzle.position, Quaternion.LookRotation(direction, Vector3.up)).GetComponent<RocketV2>();

        launchedRocket.TargetDir = direction;

		//These are now set in inspector: Resources/RocketV2
			//launchedRocket.damage = Damage;
			//launchedRocket.speed = rocketSpeed;
			//launchedRocket.areaOfEffect = rocketAreaOfEffect;
			//launchedRocket.lifeTime = rocketLifeTime;

		AmmoInMagazine--;
	}
}