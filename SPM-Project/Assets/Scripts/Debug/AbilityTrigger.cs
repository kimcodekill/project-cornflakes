using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTrigger : DebugTool
{
    public WeaponBase[] weapons;

    protected override void OnUpdate()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString())) { weapons[i].GetComponent<WeaponBase>().Trigger(); }
        }
    }
}
