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
		Ray r = Camera.main.GetComponent<Camera>().ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
		Physics.Raycast(r, out RaycastHit rhit, float.MaxValue, bulletMask);
		Vector3 direction = rhit.point - Camera.main.transform.position;
		direction /= direction.magnitude;

		Vector3 spreadForward = AddSpread(direction);

        if (Physics.Raycast(Camera.main.transform.position, spreadForward, out RaycastHit hit, range, bulletMask))
        {
            objectPooler.SpawnFromPool("Rocket", Camera.main.transform.position, Quaternion.Euler(direction)).GetComponent<Rocket>()
                        .SetTarget(hit.point, direction);
            if (bulletDebug) { DrawBulletDebug(hit); }
        }
        else
        {
            //Debug.Log("Didnt hit");

            objectPooler.SpawnFromPool("Rocket", Camera.main.transform.position, Quaternion.Euler(direction)).GetComponent<Rocket>()
				.SetTarget(hit.point, hit.normal);
        }

        //Camera.main.transform.forward = spreadForward + Camera.main.transform.forward;
        currentBulletsMagazine--;
    }
}
