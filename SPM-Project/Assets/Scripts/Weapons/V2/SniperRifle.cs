using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
/// <summary>
/// The sniper rifle has the ability to aim down sights.
/// </summary>
public class SniperRifle : Weapon {

	#region Properties

	/// <summary>
	/// The amount the camera should zoom in if the weapon is being aimed down sights with.
	/// </summary>
	public float ZoomFactor { get => zoomFactor; protected set => zoomFactor = value; }

	#endregion

	#region Serialized

	[Header("Sniper Rifle Attributes")]
	[SerializeField] private float zoomFactor;

	#endregion

	private bool wasAiming;

	protected override void Fire() {
		RaycastHit hit = MuzzleCast();
		if (hit.collider != null) {
			EventSystem.Current.FireEvent(new HitEvent() {
				Description = this + " hit " + hit.collider.gameObject,
				Source = gameObject,
				Target = hit.collider.gameObject,
				HitPoint = hit.point
			});
		}
		AmmoInMagazine--;
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Mouse1) && !wasAiming) {
			EventSystem.Current.FireEvent(new WeaponAimingDownSightsEvent() {
				IsAiming = true,
				ZoomFactor = zoomFactor
			});
			wasAiming = true;
		}
		if (Input.GetKeyUp(KeyCode.Mouse1) && wasAiming) {
			EventSystem.Current.FireEvent(new WeaponAimingDownSightsEvent() {
				IsAiming = false,
				ZoomFactor = zoomFactor
			});
			wasAiming = false;
		}
	}

	public override string ToString() {
		return "Sniper Rifle";
	}

}