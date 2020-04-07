using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPrimary : WeaponBase
{
    public override void Trigger()
    {
        //Do ammo checks first
        Shoot();        
    }

    private void Shoot()
    {
        if (Physics.Raycast(Camera.main.transform.position, AddSpread(Camera.main.transform.forward), out RaycastHit hit, range, bulletMask))
        {
            if (bulletDebug) { DrawBulletDebug(hit); }
        }
    }

    private Vector3 AddSpread(Vector3 v)
    {
        return new Vector3(Random.Range(-spread, spread) + v.x, Random.Range(-spread, spread) + v.y, Random.Range(-spread, spread) + v.z).normalized;
    }
}
