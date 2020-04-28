using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyState/EnemySearchingState")]
public class EnemySearchingState : EnemyBaseState
{
	public override void Enter() {
		Debug.Log("searching");
		Enemy.StartSearchBehaviour();
	}

	public override void Run() {
		base.Run();
		if (Enemy.FinishedSearching == true) {
			if (Enemy.isPatroller) StateMachine.TransitionTo<EnemyPatrollingState>();
			else StateMachine.TransitionTo<EnemyIdleState>();
		}
	}

	public override void Exit() {
		Enemy.StopSearchBehaviour();
	}


}
