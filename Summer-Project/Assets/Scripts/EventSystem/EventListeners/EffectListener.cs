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
        EventSystem.Current.RegisterListener<BulletHitDataEffectEvent>(OnBulletHitData);
    }

    private void OnBulletHit(Event e)
    {
        BulletHitEffectEvent bhee = e as BulletHitEffectEvent;
        ObjectPooler.Instance.SpawnFromPool("BulletHoleDecal", bhee.WorldPosition, Quaternion.identity).transform.forward = bhee.Forward;

        //Instantiate(bhee.HitEffect, bhee.WorldPosition, Quaternion.identity).transform.forward = bhee.Forward;
    }

    private void OnBulletHitData(Event e)
    {
        BulletHitDataEffectEvent bhdee = e as BulletHitDataEffectEvent;
        RaycastHit hit = bhdee.HitData;

        GameObject g = ObjectPooler.Instance.SpawnFromPool("BulletHoleDecal", hit.point, Quaternion.identity);
        g.transform.forward = -hit.normal;
        //print("pre: " + DebugStatic.StringifyV3(g.transform.localScale));

        g.transform.parent = hit.transform;

        //g.transform.localScale = s;
        //print("post: " + DebugStatic.StringifyV3(g.transform.localScale));
    }

    private void OnExplosion(Event e)
    {
        ExplosionEffectEvent eee = e as ExplosionEffectEvent;

        Instantiate(eee.ExplosionEffect, eee.WorldPosition, Quaternion.identity).transform.forward = eee.Forward;
    }
}
