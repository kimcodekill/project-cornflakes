using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPrimary : WeaponBase
{
	private void Update() {
		
		Debug.DrawRay(transform.position, Camera.main.transform.forward * 20);
		
	}

    protected override void Shoot()
    {
        Vector3 spreadForward = AddSpread(Camera.main.transform.forward);

        if (Physics.Raycast(Camera.main.transform.position, spreadForward, out RaycastHit hit, range, bulletMask))
        {
            if (bulletDebug) { DrawBulletDebug(hit); }
			if (hit.collider.gameObject.CompareTag("Target")) hit.collider.gameObject.GetComponent<SimpleEnemy>().TakeDamage(damage);
        }

        //Camera.main.transform.forward = spreadForward + Camera.main.transform.forward;

        currentBulletsMagazine--;
    }
}
