using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

//Author: Joakim Linna

public abstract class EffectEvent : Event
{
    protected EffectEvent(Vector3 worldPosition, Quaternion rotation, float scale)
    {
        WorldPosition = worldPosition;
        Rotation = rotation;
        Scale = scale;
    }

    public readonly Vector3 WorldPosition;
    public readonly Quaternion Rotation;
    public readonly float Scale;
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

public class BulletHitEffectEvent : EffectEvent
{
    public BulletHitEffectEvent(GameObject hitEffect, Vector3 worldPosition, Quaternion rotation, float scale) : base(worldPosition, rotation, scale) 
    {
        HitEffect = hitEffect;
    }

    public readonly GameObject HitEffect;
}

public class ExplosionEffectEvent : EffectEvent
{
    public ExplosionEffectEvent(GameObject explosionEffect, Vector3 worldPosition, Quaternion rotation, float scale) : base(worldPosition, rotation, scale) 
    {
        ExplosionEffect = explosionEffect;
    }

    public readonly GameObject ExplosionEffect;
}
