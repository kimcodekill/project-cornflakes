using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleSoldier : EnemySoldier
{
	private void Awake() {
		IsPatroller = false;
		base.Awake();
	}
}
