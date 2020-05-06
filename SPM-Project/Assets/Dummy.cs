using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, IEntity
{
    [SerializeField] private float maxHealth;

    private float currentHealth;
    private float deathTime;

    private bool IsDead { get { return currentHealth <= 0; } }

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public float Heal(float amount)
    {
        return currentHealth = maxHealth;
    }

    public float TakeDamage(float amount)
    {
        return currentHealth -= amount;
    }

    void Update()
    {
        if (IsDead)
        {
            if (Time.time - deathTime > 3f)
            {
                Heal(maxHealth);
            }
        }
    }
}
