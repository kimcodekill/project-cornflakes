using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/FallingState")]
public class PlayerFallingState : PlayerAirState {

	public override void Enter() {
		DebugManager.UpdateRow("STM", "PFS");
	}

}