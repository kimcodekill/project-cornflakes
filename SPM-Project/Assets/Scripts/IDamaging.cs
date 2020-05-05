using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
//Co Author: Joakim Linna

public interface IDamaging {

	float GetDamage();
	float GetExplosionDamage(Vector3 explosionCenter, Vector3 hitPos);
}