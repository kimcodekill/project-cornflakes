using System.Collections;
using UnityEngine;
using UnityEngine.AI;

//Author: Erik Pilström
public class NavMeshEnemy : EnemyBase
{

	[Header("NavAgent vars")]
	[SerializeField] [Tooltip("How fast the agent should move.")] protected float agentMoveSpeed;
	[SerializeField] [Tooltip("The agent's avoidance radius while not attacking.")] protected float defaultAgentAvoidanceRadius;
	[SerializeField] [Tooltip("The agent's avoidance radius while attacking.")] protected float attackingAgentAvoidanceRadius;
	protected NavMeshAgent agent;

	[Header("AI/behaviour vars")]
	[SerializeField] protected Transform[] waypoints;
	[SerializeField] private float alertRange;
	protected int currentWaypoint = 0;
	private float waitForSpotTime = 3f;
	protected bool wasHurtRecently;


	protected new void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		base.Awake();
	}

	protected new void Start()
	{
		RegEventListeners();
		wasHurtRecently = false;
		agent.speed = agentMoveSpeed;
		base.Start();

	}


	/// <summary>
	/// Returns the agent to its spawn point.
	/// </summary>
	protected void GoToGuardPoint()
	{
		agent.destination = Origin;
	}

	/// <summary>
	/// Simple patrol from point to point.
	/// </summary>
	protected void GoToNextPoint()
	{
		if (waypoints.Length == 0)
			return;
		agent.destination = waypoints[currentWaypoint].position;
		currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
	}

	/// <summary>
	/// Smoothing for the change in avoidance radius between the different states to avoid snapping.
	/// </summary>
	protected IEnumerator AvoidanceRadiusLerp(float targetAvoidance)
	{
		while (agent.radius != targetAvoidance)
		{
			agent.radius = Mathf.MoveTowards(agent.radius, targetAvoidance, Time.deltaTime);
			yield return null;
		}
	}

	protected override IEnumerator GradualLookAtPlayer()
	{
		
		while (Vector3.Angle(transform.forward, vectorToPlayer) > 5)
		{
			transform.forward = Vector3.RotateTowards(transform.forward, vectorToPlayer, Time.deltaTime * 5f, 0f);
			yield return null;
		}
		wasHurtRecently = false;
		//Debug.Log("" + gameObject.transform.parent.gameObject + " is looking at player");
	}

	/// <summary>
	/// Finds a new random point on the NavMesh within range, used for the Soldier's search behaviour.
	/// </summary>
	/// <param name="startPos">The origin point for the area.</param>
	/// <param name="range">How large of a sphere should be used to find the random point.</param>
	/// <returns></returns>
	protected Vector3 FindNewRandomNavMeshPoint(Vector3 startPos, float range)
	{
		Vector3 newPoint = Random.insideUnitSphere * range;
		newPoint = startPos + newPoint;
		Vector3 finalPosition = startPos + Vector3.zero;
		if (NavMesh.SamplePosition(newPoint, out NavMeshHit hit, range + agent.height, NavMesh.AllAreas))
		{
			finalPosition = hit.position;
		}
		return finalPosition;
	}

	protected void WasHurt(Event e)
	{
		EnemyHurt he = (EnemyHurt)e;
		if (he.Entity.Equals(this))
		{
			if (!isInCombat && !wasHurtRecently)
			{
				wasHurtRecently = true;
				ReceiveAlert();
			}
		}
	}

	private void ReceiveAlert()
	{
		visionRange = 100f;
		StopAllCoroutines();
		agent.ResetPath();
		StartCoroutine(GradualLookAtPlayer());
	}

	public void ReactToNearbyAlerted(Event e)
	{
		EnemyAlertEvent eae = (EnemyAlertEvent)e;
		if (!eae.AlertedEnemy.Equals(this) /*&& this.gameObject.activeInHierarchy == true*/)
		{
			float distance = Vector3.Distance(transform.position, eae.AlertedEnemy.gameObject.transform.position);
			if (distance < alertRange)
			{
				if (CanSeeTarget(GetVectorFromAtoB(transform, eae.AlertedEnemy.gameObject.transform)))
				{
					ReceiveAlert();
					StartCoroutine(WaitToSeePlayer());
				}
			}
		}
	}

	private IEnumerator WaitToSeePlayer()
	{
		yield return new WaitForSeconds(waitForSpotTime);
		if (!isInCombat)
		{
			visionRange = defaultVisionRange;
			this.StartPatrolBehaviour();
		}
	}

	protected override void RegEventListeners()
	{
		EventSystem.Current.RegisterListener<EnemyHurt>(WasHurt);
		EventSystem.Current.RegisterListener<EnemyAlertEvent>(ReactToNearbyAlerted);
	}

	public override void UnRegEventListeners()
	{
		EventSystem.Current.UnRegisterListener<EnemyAlertEvent>(ReactToNearbyAlerted);
		EventSystem.Current.UnRegisterListener<EnemyHurt>(WasHurt);
	}
}
