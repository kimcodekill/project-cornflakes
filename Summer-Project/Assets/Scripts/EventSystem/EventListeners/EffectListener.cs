using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Joakim Linna

//This should use objectpool, not instantiate
public class EffectListener : MonoBehaviour
{
    void Start()
    {
        EventSystem.Current.RegisterListener<BulletHitEffectEvent>(OnBulletHit);
        EventSystem.Current.RegisterListener<ExplosionEffectEvent>(OnExplosion);
    }

    private void OnBulletHit(Event e)
    {
        BulletHitEffectEvent bhee = e as BulletHitEffectEvent;
        ObjectPooler.Instance.SpawnFromPool("BulletHoleDecal", bhee.WorldPosition, Quaternion.identity).transform.forward = bhee.Forward;

        //Instantiate(bhee.HitEffect, bhee.WorldPosition, Quaternion.identity).transform.forward = bhee.Forward;
    }

    
    private void OnExplosion(Event e)
    {
        ExplosionEffectEvent eee = e as ExplosionEffectEvent;

        Instantiate(eee.ExplosionEffect, eee.WorldPosition, Quaternion.identity).transform.forward = eee.Forward;
    }
}
