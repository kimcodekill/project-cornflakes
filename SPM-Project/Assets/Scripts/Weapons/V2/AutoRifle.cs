using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	}

}