using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour, IPawn
{
    protected float maxHealth;
    protected float currentHealth;

    public float Heal(float amount)
    {
        throw new System.NotImplementedException();
    }

    public float TakeDamage(float amount)
    {
        throw new System.NotImplementedException();
    }
}
