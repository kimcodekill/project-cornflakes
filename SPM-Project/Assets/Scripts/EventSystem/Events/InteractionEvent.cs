using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public abstract class InteractionEvent : Event {

	public GameObject Source;

	public GameObject Target;

}

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

public class DamageEvent : InteractionEvent
{
	public IEntity Entity;
	public IDamaging Damager;
}

public abstract class PickUpEvent : InteractionEvent {

}

public class WeaponPickUpEvent : PickUpEvent
{
	public Weapon Weapon;
}

public class AmmoPickUpEvent : PickUpEvent
{
	public Weapon.EAmmoType AmmoType;
	public int AmmoAmount;
}