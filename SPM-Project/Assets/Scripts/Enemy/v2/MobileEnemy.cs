using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Author: Erik Pilström
public class MobileEnemy : EnemyBase
{
	[Header("NavAgent vars")]
	NavMeshAgent agent;
	[SerializeField] [Tooltip("How fast the agent should move.")] float agentMoveSpeed;
	[SerializeField] [Tooltip("The agent's avoidance radius while not attacking.")] float defaultAgentAvoidanceRadius;
	[SerializeField] [Tooltip("The agent's avoidance radius while attacking.")] float attackingAgentAvoidanceRadius;

	
}
