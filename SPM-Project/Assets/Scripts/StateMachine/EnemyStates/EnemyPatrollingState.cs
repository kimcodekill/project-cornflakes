using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Erik Pilström
[CreateAssetMenu(menuName = "EnemyState/EnemyPatrollingState")]
public class EnemyPatrollingState : EnemyBaseState
{
	public override void Enter() {
		Enemy.StartPatrolBehaviour();
	}

	public override void Run() {
		base.Run();
	}

	public override void Exit() {
		Enemy.StopPatrolBehaviour();
	}


}
