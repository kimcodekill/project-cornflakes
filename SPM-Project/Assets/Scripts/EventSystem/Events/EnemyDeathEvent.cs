using UnityEngine;

public class EnemyDeathEvent : Event {

	public EnemyDeathEvent(GameObject deadObject, float scoreValue)
	{
		DeadObject = deadObject;
		ScoreValue = scoreValue;
	}

	public GameObject DeadObject { get; }

	public float ScoreValue { get; }

}