using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
		Vector3 cameraHit = PlayerCamera.Instance.CameraLookPoint(BulletHitMask); // AddSpread(GetCrosshairDirection()); 

		if (cameraHit != Vector3.zero)
		{

			Vector3 aimDir = (cameraHit - Muzzle.position).normalized;

			Vector3 shotDir = AddSpread(aimDir);


			RaycastHit hit = MuzzleCast(shotDir);

			if (hit.collider != null)
			{
				IEntity entity = hit.collider.GetComponent<IEntity>();

				

				if (entity != null)
                {
					EventSystem.Current.FireEvent(new DamageEvent(entity, this));
				}
				else
				{
					//EventSystem.Current.FireEvent(new BulletHitEffectEvent(HitDecal, hit.point, -hit.normal));
				}

				EventSystem.Current.FireEvent(new BulletHitDataEffectEvent(HitDecal, hit.point, -hit.normal, hit));


				RenderBullet(this, hit.point);
			}
		}
		else 
		{ 
			RenderBullet(this, Muzzle.position + PlayerCamera.Instance.transform.forward * 100f);
        }

		RenderMuzzleFlash();

		AmmoInMagazine--;
		if (AmmoInReserve < MagazineSize) AmmoInReserve = MagazineSize;
	}
	public override string ToString() {
		return "Auto Rifle";
	}
}