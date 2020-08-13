using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

//Author: Joakim Linna

public abstract class EffectEvent : Event
{
    protected EffectEvent(Vector3 worldPosition, Vector3 forward)
    {
        WorldPosition = worldPosition;
        Forward = forward;
    }

    public readonly Vector3 WorldPosition;
    public readonly Vector3 Forward;
}

public class BulletHitEffectEvent : EffectEvent
{
    public BulletHitEffectEvent(GameObject hitEffect, Vector3 worldPosition, Vector3 forward) : base(worldPosition, forward) 
    {
        HitEffect = hitEffect;
    }

    public readonly GameObject HitEffect;

}

public class BulletHitDataEffectEvent : EffectEvent
{
    public BulletHitDataEffectEvent(GameObject hitEffect, Vector3 worldPosition, Vector3 forward, RaycastHit hitData) : base(worldPosition, forward)
    {
        HitEffect = hitEffect;
        HitData = hitData;
    }

    public readonly GameObject HitEffect;
    public readonly RaycastHit HitData;
}

public class ExplosionEffectEvent : EffectEvent
{
    public ExplosionEffectEvent(GameObject explosionEffect, Vector3 worldPosition, Vector3 forward, float scale) : base(worldPosition, forward) 
    {
        ExplosionEffect = explosionEffect;
    }

    public readonly GameObject ExplosionEffect;
    public readonly float scale;
}
