using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyState/EnemyIdleState")]
public class EnemyIdleState : EnemyBaseState
{

	public override void Enter() {
		Debug.Log("idling");
		Enemy.StartIdleBehaviour();
	}

	public override void Run() {
		base.Run();
	}

	public override void Exit() {
		Enemy.StopIdleBehaviour();
	}


}
