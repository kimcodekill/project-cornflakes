using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IPawn
{
    [SerializeField] protected float health = 100;
    [SerializeField] protected float viewRange = 25;
    [SerializeField] protected float fieldOfView = 170;
    [SerializeField] protected WeaponBase weapon;

    public float Heal(float amount)
    {
        health += amount;
        return health;
    }

    protected bool PlayerInRange()
    {
        return (PlayerController.PlayerInstance.transform.position - transform.position).magnitude <= viewRange;
    }

    protected RaycastHit PlayerSeen()
    {
        //Physics.Raycast();
        return new RaycastHit();
    }

    public float TakeDamage(float amount)
    {
        health -= amount;
        return health;
    }

}
