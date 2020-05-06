using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Erik Pilström
public class PatrollingSoldier : EnemySoldier
{
	[SerializeField] [Tooltip("The patrol points. NEVER USE ONLY ONE.")] public Transform[] patrolPoints;
	//Seriously, never have only one. If you want a non-patrolling soldier, use the IdleSoldier instead.

	private void Awake() {
		IsPatroller = true; //Sets that the Soldier this script is attached to should be a patrolling Soldier.
		base.Awake();
	}
}
