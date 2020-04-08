using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPawn
{
    float TakeDamage(float amount);

    float Heal(float amount);
}