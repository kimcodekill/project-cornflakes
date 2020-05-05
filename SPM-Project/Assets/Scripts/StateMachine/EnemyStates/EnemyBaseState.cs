using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Erik Pilström
public abstract class EnemyBaseState : State
{
    private Enemy enemy;
    public Enemy Enemy => enemy = enemy != null ? enemy : (Enemy)Owner;

	public override void Run() {
		if (Enemy.TargetIsInSight()) { StateMachine.TransitionTo<EnemyAlertedState>(); }
	}

}
