using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpecial : WeaponBase
{
    [SerializeField] [Tooltip("Pellet Amount per Shell")] private float pelletCount;

    protected override void Shoot()
    {
        Vector3 spreadForward;
        Vector3 recoil = Vector3.zero;
        IEntity pawn;

        for (int i = 0; i < pelletCount; i++)
        {
            spreadForward = AddSpread(Camera.main.transform.forward);
            recoil += spreadForward;
            
            if(Physics.Raycast(Camera.main.transform.position, spreadForward, out RaycastHit hit, range, bulletMask))
            {
                HurtPawn(hit.collider.gameObject.GetComponentInParent<IEntity>());
                if (bulletDebug) { DrawBulletDebug(hit); }
			}
        }

        //Camera.main.transform.forward = (recoil / pelletCount) + Camera.main.transform.forward;

        currentBulletsMagazine--;
    }
}
