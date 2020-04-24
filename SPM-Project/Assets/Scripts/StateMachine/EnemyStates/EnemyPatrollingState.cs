using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyState/EnemyPatrollingState")]
public class EnemyPatrollingState : EnemyBaseState
{
	public override void Enter() {
		Debug.Log("" + Enemy.gameObject.name + " patrolling");
		Enemy.StartPatrolBehaviour();
	}

	public override void Run() {
		base.Run();
	}

	public override void Exit() {
		Enemy.StopPatrolBehaviour();
	}


}
