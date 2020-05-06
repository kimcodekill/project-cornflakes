using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public abstract class WeaponEvent : Event {

	public GameObject GameObject;

	public Weapon Weapon { get => weapon == null ? weapon = GameObject.GetComponent<Weapon>() : weapon; }

	private Weapon weapon;

}

public class WeaponFiredEvent : WeaponEvent {

}

public class WeaponReloadingEvent : WeaponEvent {

}

public class WeaponAimingDownSightsEvent : Event {

	public bool IsAiming;

	public float ZoomFactor;

}