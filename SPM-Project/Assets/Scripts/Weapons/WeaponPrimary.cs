using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPrimary : WeaponBase
{
    public override void Trigger()
    {
        if (shotDelay >= fireRate * Time.deltaTime)
        {
            Shoot();
            shotDelay = 0;
        }
    }

    private void Shoot()
    {
        Vector3 spreadForward = AddSpread(Camera.main.transform.forward);

        if (Physics.Raycast(Camera.main.transform.position, spreadForward, out RaycastHit hit, range, bulletMask))
        {
            Camera.main.transform.forward = spreadForward + Camera.main.transform.forward;

            if (bulletDebug) { DrawBulletDebug(hit); }
        }

        currentBulletsMagazine--;
    }

    private Vector3 AddSpread(Vector3 v)
    {
        return new Vector3(Random.Range(-spread, spread) + v.x, Random.Range(-spread, spread) + v.y, Random.Range(-spread, spread) + v.z).normalized;
    }
}
