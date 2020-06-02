using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dummy : MonoBehaviour, IEntity
{
    [SerializeField] private float maxHealth;
    [SerializeField] private TextMeshPro healthText;

    private float currentHealth;
    private float deathTime;

    private bool IsDead { get { return currentHealth <= 0; } }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Die()
    {
        EventSystem.Current.FireEvent(new EnemyDeathEvent(gameObject, maxHealth));

        deathTime = Time.time;
    }

    public float Heal(float amount)
    {
        deathTime = 0;
        return currentHealth = maxHealth;
    }

    public float TakeDamage(float amount, DamageType damageType)
    {
        if (!IsDead)
        {
            currentHealth -= amount;

            if (IsDead) Die();
        }

        return currentHealth;
    }

    void Update()
    {
        if (IsDead && Time.time - deathTime > 3f)
        {
            Heal(maxHealth);
        }

        healthText.text = currentHealth + " HP";
    }
}
