﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrollingState : EnemyBaseState
{
	[SerializeField] [Tooltip("The patrol points.")] private Transform[] points;

	public override void Run() {
		///do patrol things
		base.Run();
	}

	private void DoPatrol() { }

}
