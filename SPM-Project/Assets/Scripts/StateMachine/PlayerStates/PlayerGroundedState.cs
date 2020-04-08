using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState {

	public override void Run() {
		if (Input.GetKeyDown(KeyCode.Space) && Player.PhysicsBody.IsGrounded()) StateMachine.Push<PlayerJumpingState>();
		
		base.Run();
	}

}