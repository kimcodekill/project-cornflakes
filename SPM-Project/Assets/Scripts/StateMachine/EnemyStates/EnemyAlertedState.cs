using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Erik Pilström
[CreateAssetMenu(menuName = "EnemyState/EnemyAlertedState")]
public class EnemyAlertedState : EnemyBaseState
{
	public override void Enter() {
		Enemy.StartAlertedBehaviour();
		//Alert other nearby enemies? Possible feature expansion

	}

	public override void Run() {
		if (Enemy.TargetIsAttackable()) { StateMachine.TransitionTo<EnemyAttackingState>(); }
		if (!Enemy.TargetIsInSight()) {
			if (Enemy is EnemySoldier) StateMachine.TransitionTo<EnemySearchingState>();
			else if (Enemy is EnemyTurret) StateMachine.TransitionTo<EnemyIdleState>();
			else StateMachine.TransitionTo<EnemyPatrollingState>();
		}
	}

	public override void Exit() {
		Enemy.StopAlertedBehaviour();
	}
}
