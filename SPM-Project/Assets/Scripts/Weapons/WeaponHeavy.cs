using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHeavy : WeaponBase
{
    ObjectPooler objectPooler;

    protected override void Awake()
    {
        objectPooler = ObjectPooler.Instance;
        base.Awake();
    }

    protected override void Shoot()
    {
        Vector3 spreadForward = AddSpread(Camera.main.transform.forward);

        if (Physics.Raycast(Camera.main.transform.position, spreadForward, out RaycastHit hit, range, bulletMask))
        {
            objectPooler.SpawnFromPool("Rocket", Camera.main.transform.position, Quaternion.Euler(Camera.main.transform.forward)).GetComponent<Rocket>()
                        .SetTarget(hit.point, hit.normal);

            if (bulletDebug) { DrawBulletDebug(hit); }
        }
        else
        {
            Debug.Log("Didnt hit");

            objectPooler.SpawnFromPool("Rocket", Camera.main.transform.position, Quaternion.Euler(Camera.main.transform.forward)).GetComponent<Rocket>()
                        .SetTarget(Camera.main.transform.forward * range, Camera.main.transform.forward);
        }

        //Camera.main.transform.forward = spreadForward + Camera.main.transform.forward;
        currentBulletsMagazine--;
    }
}
