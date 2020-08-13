using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour, IPooledObject
{
    public ParticleSystem smokeSystem;
    public ParticleSystem dustSystem;

    public void OnObjectSpawn()
    {
        smokeSystem.Play();
        dustSystem.Play();
    }
}
