using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRifle : Weapon {

	public override void Fire() {
		Vector3 direction = AddSpread(GetDirectionToPoint(Muzzle.forward, GetCrosshairHitPoint()));

		EventSystem.Current.FireEvent(new WeaponFiredEvent() {
			Description = gameObject + " fired a shot",
			GameObject = gameObject
		});

		if (Physics.Raycast(Muzzle.forward, direction, out RaycastHit hit, float.MaxValue, BulletHitMask)) {
			EventSystem.Current.FireEvent(new HitEvent() {
				Description = gameObject + " hit " + hit.collider.gameObject,
				Source = gameObject,
				Target = hit.collider.gameObject
			});
		}

		AmmoInMagazine--;
	}

}