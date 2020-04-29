using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingSoldier : EnemySoldier
{
	[SerializeField] [Tooltip("The patrol points. NEVER USE ONLY ONE.")] public Transform[] points;
	//Seriously, never have only one. If you want a non-patrolling soldier, use the IdleSoldier prefab instead.

	private void Awake() {
		IsPatroller = true;
		base.Awake();
	}
}
