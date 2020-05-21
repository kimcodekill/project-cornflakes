using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
/// <summary>
/// The shotgun fires shells, containing some amount of pellets per shot.
/// </summary> 
[CreateAssetMenu(menuName = "Weapon/Shotgun")]
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
		for (int i = 0; i < pelletCount; i++) 
		{
			RaycastHit hit = MuzzleCast();
			if (hit.collider != null) 
			{
				EventSystem.Current.FireEvent(new DamageEvent(hit.collider.gameObject.GetComponent<IEntity>(), this));

				EventSystem.Current.FireEvent(new BulletHitEffectEvent(HitDecal, hit.point, Quaternion.identity, 1.0f));
			}
		}

		AmmoInMagazine--;
	}

	public override string ToString() {
		return "Shotgun";
	}

}