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
		//Debug.Log("alerted run");
		//Debug.Log(Enemy.PlayerIsInSight());
		if(!Enemy.PlayerIsInSight()) { StateMachine.Pop(); }
		if (Enemy.TargetIsAttackable()) { StateMachine.Push<EnemyAttackingState>(); }
		///Perhaps give some sort of time where the enemy stays alerted, doing things like searching for the player with an EnemySearchingState or similar
	}

	public override void Exit() {
		Enemy.StopAlertedBehaviour();
	}
}
