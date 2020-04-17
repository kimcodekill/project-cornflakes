using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The auto rifle fires automatically, has infinite ammo but can overheat.
/// If AmmoInMagazine is 0, then the weapon has overheated.
/// The reload time here becomes the "cooldown" stage.
/// Ammo in reserve is irrelevant, so we refill it all the time.
/// </summary>
public class AutoRifle : Weapon {

	protected override void Fire() {
		RaycastHit hit = MuzzleCast();
		if (hit.collider != null) {
			EventSystem.Current.FireEvent(new HitEvent() {
				Description = this + " hit " + hit.collider.gameObject,
				Source = gameObject,
				Target = hit.collider.gameObject
			});
		}
		AmmoInMagazine--;
		if (AmmoInReserve < MagazineSize) AmmoInReserve = MagazineSize;
	}

}