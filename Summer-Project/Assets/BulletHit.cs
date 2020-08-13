using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class BulletHit : MonoBehaviour, IPooledObject
{
    public ParticleSystem smokeSystem;
    public ParticleSystem dustSystem;

    Vector3 startScale;

    private void OnEnable()
    {
        if (startScale == Vector3.zero)
        {
            startScale = transform.localScale;
        }
    }



    public void OnObjectSpawn()
    {
        transform.parent = null;
        transform.localScale = startScale;
        smokeSystem.Play();
        dustSystem.Play();
    }
}
