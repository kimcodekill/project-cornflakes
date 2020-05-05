using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Joakim Linna

//This should use objectpool, not instantiate
public class EffectListener : MonoBehaviour
{
    void Start()
    {
        EventSystem.Current.RegisterListener<BulletEffectEvent>(OnBulletHit);
        EventSystem.Current.RegisterListener<ExplosionEffectEvent>(OnExplosion);
    }

    private void OnBulletHit(Event e)
    {
        BulletEffectEvent bee = e as BulletEffectEvent;

        Instantiate(bee.HitEffect, bee.WorldPosition, bee.Rotation);
    }

    
    private void OnExplosion(Event e)
    {
        ExplosionEffectEvent eee = e as ExplosionEffectEvent;

        Instantiate(eee.ExplosionEffect, eee.WorldPosition, eee.Rotation).transform.localScale *= eee.Scale;
    }
}
