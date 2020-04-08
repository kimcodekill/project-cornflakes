using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHeavy : WeaponBase
{
    [SerializeField] private Rocket[] rockets;

    private int iterator = 0;

    protected override void Shoot()
    {
        Vector3 spreadForward = AddSpread(Camera.main.transform.forward);

        if (Physics.Raycast(Camera.main.transform.position, spreadForward, out RaycastHit hit, range, bulletMask))
        {
            rockets[iterator].Send(hit.transform, spreadForward);
            if (bulletDebug) { DrawBulletDebug(hit); }

        }
        else
        {
            rockets[iterator].Send(Camera.main.transform.forward * range, spreadForward);
        }

        //Camera.main.transform.forward = spreadForward + Camera.main.transform.forward;
        currentBulletsMagazine--;

        if (++iterator == rockets.Length) { iterator = 0; }
    }
}
