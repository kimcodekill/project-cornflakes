using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public abstract class InteractionEvent : Event {

	public GameObject Source;

	public GameObject Target;

}

public class HitEvent : InteractionEvent {

	public Vector3 HitPoint;

}

public class DamageEvent : InteractionEvent {

	public float Damage;

}

public class PickUpEvent : InteractionEvent {

}