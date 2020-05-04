using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyState/EnemyAlertedState")]
public class EnemyAlertedState : EnemyBaseState
{
	public override void Enter() {
		//Debug.Log("alerted");
		Enemy.StartAlertedBehaviour();
		///alert other nearby enemies???

	}

	public override void Run() {
		if (Enemy.TargetIsAttackable()) { StateMachine.TransitionTo<EnemyAttackingState>(); }
		if (!Enemy.PlayerIsInSight()) {
			if (Enemy is EnemySoldier) StateMachine.TransitionTo<EnemySearchingState>();
			else if (Enemy is EnemyTurret) StateMachine.TransitionTo<EnemyIdleState>();
			else StateMachine.TransitionTo<EnemyPatrollingState>();
		}
	}

	public override void Exit() {
		Enemy.StopAlertedBehaviour();
	}
}
