using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyState/EnemyAlertedState")]
public class EnemyAlertedState : EnemyBaseState
{
	public override void Enter() {
		///alert other nearby enemies???

	}

	public override void Run() {
		
		if(!Enemy.PlayerIsInSight()) { StateMachine.Pop(); }
		
		if (Enemy.TargetIsAttackable()) { StateMachine.Push<EnemyAttackingState>(); }
		///Perhaps give some sort of time where the enemy stays alerted, doing things like searching for the player with an EnemySearchingState or similar
	}

	
}
