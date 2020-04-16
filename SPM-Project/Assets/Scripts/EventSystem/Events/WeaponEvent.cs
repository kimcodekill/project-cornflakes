using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponEvent : Event {

	public GameObject GameObject;

}

public class WeaponFiredEvent : WeaponEvent {

}

public class WeaponReloadingEvent : WeaponEvent {

}