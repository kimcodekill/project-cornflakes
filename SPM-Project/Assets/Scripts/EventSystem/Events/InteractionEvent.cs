using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public abstract class InteractionEvent : Event {

	[Obsolete("If you're using this, you're doing something wrong.", true)]
	public GameObject Source;
	[Obsolete("If you're using this, you're doing something wrong.", true)]
	public GameObject Target;

}

public class DamageEvent : InteractionEvent
{
	public DamageEvent(IEntity entity, IDamaging damager)
	{
		Entity = entity;
		Damager = damager;
	}

	public readonly IEntity Entity;
	public readonly IDamaging Damager;

}

public class ExplosiveDamageEvent : DamageEvent
{
	public ExplosiveDamageEvent(IEntity entity, IDamaging damager, float damageScale ) : base(entity, damager)
	{
		DamageScale = Mathf.Clamp(damageScale, 0.0f, 1.0f);
	}

	public readonly float DamageScale;
}

#region HitEvent

public class HitEvent : InteractionEvent {

	public Vector3 HitPoint;

}

public class BulletHitEvent : HitEvent
{
	public Weapon Weapon;
}

public class ExplosionHitEvent : HitEvent
{

}

#endregion

#region PickUpEvent

public abstract class PickUpEvent : InteractionEvent {

	protected PickUpEvent(GameObject pickup, GameObject other)
	{
		Pickup = pickup;
		Other = other;
	}

	public readonly GameObject Pickup;
	public readonly GameObject Other;
}

public class WeaponPickUpEvent : PickUpEvent
{
	public WeaponPickUpEvent(GameObject pickup, GameObject other, Weapon weapon) : base(pickup, other)
	{
		Weapon = weapon;
	}

	public readonly Weapon Weapon;
}

public class AmmoPickUpEvent : PickUpEvent
{
	public AmmoPickUpEvent(GameObject pickup, GameObject other, Weapon.EAmmoType ammoType, int ammoAmount) : base(pickup, other)
	{
		AmmoType = ammoType;
		AmmoAmount = ammoAmount;
	}
	public readonly Weapon.EAmmoType AmmoType;
	public readonly int AmmoAmount;
}

#endregion