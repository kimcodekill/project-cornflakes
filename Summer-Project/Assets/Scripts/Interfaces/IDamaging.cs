using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
//Co Author: Joakim Linna

public interface IDamaging {
	
	float GetDamage();
}

public enum DamageType
{
	Bullet,
	Explosive,
	Instant
}