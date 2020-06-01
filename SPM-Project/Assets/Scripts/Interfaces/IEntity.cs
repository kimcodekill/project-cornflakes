using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Joakim Linna
//Co author: Viktor Dahlberg

public interface IEntity {
    
	float TakeDamage(float amount, DamageType damageType);

    float Heal(float amount);

}