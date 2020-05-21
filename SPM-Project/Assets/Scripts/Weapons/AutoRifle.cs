using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
/// <summary>
/// The auto rifle fires automatically, has infinite ammo but can overheat.
/// If AmmoInMagazine is 0, then the weapon has overheated.
/// The reload time here becomes the "cooldown" stage.
/// Ammo in reserve is irrelevant, so we refill it all the time.
/// </summary>
[CreateAssetMenu(menuName = "Weapon/Auto Rifle")]
public class AutoRifle : Weapon {

	#region Properties

	public float CooldownWait { get => cooldownWait; set => cooldownWait = value; }
	
	public float CurrentCooldownTime { get; set; }

	#endregion

	#region Serialized

	[SerializeField] private float cooldownWait;

	#endregion

	protected override void Fire() 
	{
		RaycastHit hit = MuzzleCast();
		if (hit.collider != null) 
		{
			EventSystem.Current.FireEvent(new DamageEvent(hit.collider.GetComponent<IEntity>(), this));

			EventSystem.Current.FireEvent(new BulletHitEffectEvent(HitDecal, hit.point, Quaternion.identity, 1.0f));
		}

		AmmoInMagazine--;
		if (AmmoInReserve < MagazineSize) AmmoInReserve = MagazineSize;
	}
	public override string ToString() {
		return "Auto Rifle";
	}
}