using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Joakim Linna

public class DebugTrigger : DebugTool
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] pickups;
    [SerializeField] Target target;

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            player.GetComponent<PlayerController>().TakeDamage(float.PositiveInfinity);
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            target.Heal(-1);
        }
    }
}
