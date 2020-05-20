using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Author: Joakim Linna

public abstract class EffectEvent : Event
{
    
}

public abstract class WeaponEffectEvent : Event
{
    public AudioClip AudioClip;
    public AudioSource AudioSource;
}

//Move to playerevent
public class WeaponFiredEvent : WeaponEffectEvent {
    
}

//Move to playerevent
public class WeaponReloadingEvent : WeaponEffectEvent {

}

//Move to playerevent
public class WeaponSwitchedEvent : WeaponEffectEvent
{
    public Weapon SelectedWeapon;
}

public abstract class HitEffectEvent : EffectEvent
{
    public GameObject HitEffect;
    public Vector3 WorldPosition;
    public Quaternion Rotation;
    public float Scale;
}

public class BulletEffectEvent : HitEffectEvent
{

}

public class ExplosionEffectEvent : HitEffectEvent
{
    public GameObject ExplosionEffect;
}
