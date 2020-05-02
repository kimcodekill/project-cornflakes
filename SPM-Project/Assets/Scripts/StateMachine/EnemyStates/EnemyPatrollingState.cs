using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyState/EnemyPatrollingState")]
public class EnemyPatrollingState : EnemyBaseState
{
	public override void Enter() {
		Debug.Log("patrolling");
		Enemy.StartPatrolBehaviour();
		//Enemy.audioSource.clip = Enemy.audioClips[0];
		Enemy.audioSource.Play();
	}

	public override void Run() {
		base.Run();
	}

	public override void Exit() {
		Enemy.StopPatrolBehaviour();
	}


}
