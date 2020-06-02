using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Erik Pilström
[CreateAssetMenu(menuName = "EnemyState/EnemySearchingState")]
public class EnemySearchingState : EnemyBaseState
{
	public override void Enter() {
		Enemy.StartSearchBehaviour();
	}

	public override void Run() {
		if (Enemy.HasFinishedSearching == true) {
			if (Enemy is MobileEnemy) StateMachine.TransitionTo<EnemyPatrollingState>();
			else StateMachine.TransitionTo<EnemyIdleState>();
		}
		base.Run();
	}

	public override void Exit() {
		if (!Enemy.HasFinishedSearching) { Enemy.StopSearchBehaviour(); } //Only calls StopSearch for the enemy if they didn't finish searching.
	}

}
