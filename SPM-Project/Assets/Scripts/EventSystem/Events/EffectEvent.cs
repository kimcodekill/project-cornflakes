using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EffectEvent : Event
{
    public GameObject HitEffect;
    public AudioClip AudioClip;
    public Vector3 WorldPosition;
    public Quaternion Rotation;
    public float Scale;
}

public class BulletEffectEvent : EffectEvent
{

}

public class ExplosionEffectEvent : EffectEvent
{
    public GameObject ExplosionEffect;
}
