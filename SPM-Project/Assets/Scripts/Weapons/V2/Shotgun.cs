using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon {

	#region Properties

	/// <summary>
	/// How many pellets the shotgun should fire per shot.
	/// </summary>
	public int PelletCount { get => pelletCount; protected set => pelletCount = value; }

	#endregion

	#region Serialized
	
	[Header("Shotgun Attributes")]
	[SerializeField] private int pelletCount;

	#endregion

	public override void Fire() {
		Vector3 direction = GetDirectionToPoint(Muzzle.position, GetCrosshairHitPoint());
		EventSystem.Current.FireEvent(new WeaponFiredEvent() {
			Description = gameObject + " fired a shot", GameObject = gameObject
		});
		
		for (int i = 0; i < pelletCount; i++) {
			if (Physics.Raycast(Muzzle.position, AddSpread(direction), out RaycastHit hit, float.MaxValue, BulletHitMask)) {
				EventSystem.Current.FireEvent(new HitEvent() {
					Description = gameObject + " hit " + hit.collider.gameObject,
					Source = gameObject,
					Target = hit.collider.gameObject
				});
			}
		}
		AmmoInMagazine--;
	}

}