using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The sniper rifle has the ability to aim down sights.
/// </summary>
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