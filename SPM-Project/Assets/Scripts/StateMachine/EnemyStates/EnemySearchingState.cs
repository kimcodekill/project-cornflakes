using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyState/EnemySearchingState")]
public class EnemySearchingState : EnemyBaseState
{
	public override void Enter() {
		//Debug.Log("searching");
		Enemy.StartSearchBehaviour();
	}

	public override void Run() {
		if (Enemy.FinishedSearching == true) {
			if (Enemy.IsPatroller) StateMachine.TransitionTo<EnemyPatrollingState>();
			else StateMachine.TransitionTo<EnemyIdleState>();
		}
		base.Run();
	}

	public override void Exit() {
		//Debug.Log("exiting search, " + "searching finished: " + Enemy.FinishedSearching);
		if (!Enemy.FinishedSearching) { Enemy.StopSearchBehaviour(); }
	}

}
