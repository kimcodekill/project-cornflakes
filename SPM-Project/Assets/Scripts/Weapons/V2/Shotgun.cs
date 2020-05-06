using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
/// <summary>
/// The shotgun fires shells, containing some amount of pellets per shot.
/// </summary>
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

	protected override void Fire() {
		for (int i = 0; i < pelletCount; i++) {
			RaycastHit hit = MuzzleCast();
			if (hit.collider != null) {
				EventSystem.Current.FireEvent(new HitEvent() {
					Description = this + " hit " + hit.collider.gameObject,
					Source = gameObject,
					Target = hit.collider.gameObject,
					HitPoint = hit.point
				});
			}
		}
		AmmoInMagazine--;
	}

	public override string ToString() {
		return "Shotgun";
	}

}