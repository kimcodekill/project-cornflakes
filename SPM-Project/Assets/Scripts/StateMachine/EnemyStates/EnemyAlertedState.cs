using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyState/EnemyAlertedState")]
public class EnemyAlertedState : EnemyBaseState
{
	public override void Enter() {
		Debug.Log("alerted");
		Enemy.StartAlertedBehaviour();
		///alert other nearby enemies???

	}

	public override void Run() {
		if (Enemy.TargetIsAttackable()) { StateMachine.TransitionTo<EnemyAttackingState>(); }
		if(!Enemy.PlayerIsInSight()) { StateMachine.TransitionTo<EnemySearchingState>(); }
	}

	public override void Exit() {
		Enemy.StopAlertedBehaviour();
	}
}
