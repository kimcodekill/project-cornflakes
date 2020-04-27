using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/FallingState")]
public class PlayerFallingState : PlayerAirState {

	public override void Enter() {
		DebugManager.UpdateRow("PlayerSTM" + Player.gameObject.GetInstanceID(), GetType().ToString());
	}

	public override void Run() {
		if (Input.GetKeyDown(KeyCode.Space)) StateMachine.Push<PlayerJumpingState>();
		base.Run();
	}

}