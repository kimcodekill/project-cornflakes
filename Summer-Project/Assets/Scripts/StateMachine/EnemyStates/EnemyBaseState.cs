using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Erik Pilström
public abstract class EnemyBaseState : State
{
    private EnemyBase enemy;
    public EnemyBase Enemy => enemy = enemy != null ? enemy : (EnemyBase)Owner;

	public override void Run() {
		if (Enemy.TargetIsInSight()) { StateMachine.TransitionTo<EnemyAlertedState>(); }
	}

}
