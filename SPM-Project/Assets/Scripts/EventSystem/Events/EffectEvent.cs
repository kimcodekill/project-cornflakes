using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Author: Joakim Linna

public abstract class EffectEvent : Event
{
    public GameObject HitEffect;
    public Vector3 WorldPosition;
    public Quaternion Rotation;
    public float Scale;
	public AudioClip AudioClip;
	public AudioSource AudioSource;
}

//Move to playerevent
public class WeaponFiredEvent : EffectEvent {

}

//Move to playerevent
public class WeaponReloadingEvent : EffectEvent {

}

//Move to playerevent
public class WeaponSwitchedEvent : EffectEvent
{
    public Weapon SelectedWeapon;
}

public class BulletEffectEvent : EffectEvent
{

}

public class ExplosionEffectEvent : EffectEvent
{
    public GameObject ExplosionEffect;
}
