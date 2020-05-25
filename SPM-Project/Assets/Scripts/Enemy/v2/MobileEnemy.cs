using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Author: Erik Pilström
public class MobileEnemy : EnemyBase
{
	[Header("NavAgent vars")]
	protected NavMeshAgent agent;
	[SerializeField] [Tooltip("How fast the agent should move.")] protected float agentMoveSpeed;
	[SerializeField] [Tooltip("The agent's avoidance radius while not attacking.")] protected float defaultAgentAvoidanceRadius;
	[SerializeField] [Tooltip("The agent's avoidance radius while attacking.")] protected float attackingAgentAvoidanceRadius;

	[Header("AI/behaviour vars")]
	[SerializeField] protected Transform[] waypoints;
	protected int currentWaypoint = 0;


	protected new void Awake() {
		agent = GetComponent<NavMeshAgent>();
		base.Awake();
	}

	/// <summary>
	/// Returns the agent to its spawn point.
	/// </summary>
	protected void GoToGuardPoint() {
		agent.destination = Origin;
	}

	/// <summary>
	/// Simple patrol from point to point.
	/// </summary>
	protected void GoToNextPoint() {
		if (waypoints.Length == 0)
			return;
		agent.destination = waypoints[currentWaypoint].position;
		currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
	}

	/// <summary>
	/// Smoothing for the change in avoidance radius between the different states to avoid snapping.
	/// </summary>
	protected IEnumerator AvoidanceRadiusLerp(float targetAvoidance) {
		while (agent.radius != targetAvoidance) {
			agent.radius = Mathf.MoveTowards(agent.radius, targetAvoidance, Time.deltaTime);
			yield return null;
		}
	}

	/// <summary>
	/// Finds a new random point on the NavMesh within range, used for the Soldier's search behaviour.
	/// </summary>
	/// <param name="startPos">The origin point for the area.</param>
	/// <param name="range">How large of a sphere should be used to find the random point.</param>
	/// <returns></returns>
	protected Vector3 FindNewRandomNavMeshPoint(Vector3 startPos, float range) {
		Vector3 newPoint = Random.insideUnitSphere * range;
		newPoint = startPos + newPoint;
		Vector3 finalPosition = startPos + Vector3.zero;
		if (NavMesh.SamplePosition(newPoint, out NavMeshHit hit, range + agent.height, NavMesh.AllAreas)) {
			finalPosition = hit.position;
		}
		return finalPosition;
	}
}
