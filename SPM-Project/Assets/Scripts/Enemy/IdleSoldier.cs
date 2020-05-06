using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Erik Pilström
public class IdleSoldier : EnemySoldier
{
	private void Awake() {
		IsPatroller = false; //Sets that the Soldier this script is attached to should be an idle Soldier.
		base.Awake();
	}
}
