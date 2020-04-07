using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTrigger : DebugTool
{
    public WeaponBase[] weapons;
    private WeaponBase currentWeapon;

    protected override void OnUpdate()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()) || Input.GetKey((i + 1).ToString())) currentWeapon = weapons[i];
        }

        if (weapons[i].GetComponent<WeaponBase>().HasAmmo())
        {
            if (Input.GetKeyDown((i + 1).ToString()) || Input.GetKey((i + 1).ToString()))
            {
                weapons[i].GetComponent<WeaponBase>().Trigger();
            }
        }
        else
        {
            weapons[i].GetComponent<WeaponBase>().DoReload();
        }    
    }
}
