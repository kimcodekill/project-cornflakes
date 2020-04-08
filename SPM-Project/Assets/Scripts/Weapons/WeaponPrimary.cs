using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPrimary : WeaponBase
{
    protected override void Shoot()
    {
        Vector3 spreadForward = AddSpread(Camera.main.transform.forward);

        if (Physics.Raycast(Camera.main.transform.position, spreadForward, out RaycastHit hit, range, bulletMask))
        {
            if (bulletDebug) { DrawBulletDebug(hit); }
        }

        Camera.main.transform.forward = spreadForward + Camera.main.transform.forward;

        currentBulletsMagazine--;
    }
}
