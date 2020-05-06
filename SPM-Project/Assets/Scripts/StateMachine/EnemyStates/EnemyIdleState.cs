using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Erik Pilström
[CreateAssetMenu(menuName = "EnemyState/EnemyIdleState")]
public class EnemyIdleState : EnemyBaseState
{

	public override void Enter() {
		Enemy.StartIdleBehaviour();
	}

	public override void Run() {
		base.Run();
	}

	public override void Exit() {
		Enemy.StopIdleBehaviour();
	}


}
