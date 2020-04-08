using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/FallingState")]
public class PlayerFallingState : PlayerAirState {

	public override void Enter() {
		DebugManager.UpdateRow("STM", "PFS");
	}

	public override void Run() {
		if (Player.PhysicsBody.IsGrounded()) {
			if (Player.GetInput().magnitude > 0) StateMachine.TransitionTo<PlayerMovingState>();
			else StateMachine.TransitionTo<PlayerStandingState>();
		}

		base.Run();
	}

}