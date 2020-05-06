using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Joakim Linna
//Co author: Viktor Dahlberg

public interface IEntity {
    
	float TakeDamage(float amount);

    float Heal(float amount);

    void Die();

}