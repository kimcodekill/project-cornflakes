using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity {
    
	float TakeDamage(float amount);

    float Heal(float amount);

}