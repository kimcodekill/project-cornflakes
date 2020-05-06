using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Author: Joakim Linna

public class Target : MonoBehaviour, IEntity
{
    [SerializeField] private TextMeshPro textAllDamage;
    [SerializeField] private TextMeshPro textDPS;
    [SerializeField] private TextMeshPro textLatestDamage;

    private float takenDamage;
    private float latestDamage;

    public void Die()
    {
        return;
    }

    public float Heal(float amount)
    {
        takenDamage = 0;
        WriteToText();
        return 0;
    }

    public float TakeDamage(float amount)
    {
        takenDamage += amount;
        latestDamage = amount;
        return amount;
    }

    private void Update()
    {
        WriteToText();
    }

    private void WriteToText()
    {
        textAllDamage.text = "Total damage: " + takenDamage;
        //textDPS.text = "Damage per second: " + (takenDamage / -(Time.deltaTime - Time.time));
        textLatestDamage.text = "Latest damage: " + latestDamage;
    }
}
