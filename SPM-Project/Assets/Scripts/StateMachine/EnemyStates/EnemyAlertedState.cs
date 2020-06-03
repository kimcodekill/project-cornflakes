using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Erik Pilström
[CreateAssetMenu(menuName = "EnemyState/EnemyAlertedState")]
public class EnemyAlertedState : EnemyBaseState
{
	public override void Enter() {
		Enemy.StartAlertedBehaviour();
		EventSystem.Current.FireEvent(new EnemyAlertEvent() { AlertedEnemy = Enemy });
	}

	public override void Run() {
		if (Enemy.TargetIsAttackable()) { StateMachine.TransitionTo<EnemyAttackingState>(); }
		if (!Enemy.TargetIsInSight()) {
			if (Enemy is Scout) StateMachine.TransitionTo<EnemySearchingState>();
			else if (Enemy is MobileEnemy) StateMachine.TransitionTo<EnemyPatrollingState>();
			else StateMachine.TransitionTo<EnemyIdleState>();
			
		}
	}

	public override void Exit() {
		Enemy.StopAlertedBehaviour();
	}
}
