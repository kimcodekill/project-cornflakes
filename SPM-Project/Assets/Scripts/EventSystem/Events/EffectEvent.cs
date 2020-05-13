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
	public Audio Audio;
}

public class WeaponFiredEvent : EffectEvent {

}

public class WeaponReloadingEvent : EffectEvent {

}
public class WeaponSwitchedEvent : EffectEvent {

}

public class BulletEffectEvent : EffectEvent
{

}

public class SFXEvent : EffectEvent {

}

public class ExplosionEffectEvent : EffectEvent
{
    public GameObject ExplosionEffect;
}
