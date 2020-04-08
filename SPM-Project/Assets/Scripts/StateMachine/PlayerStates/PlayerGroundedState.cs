using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState {

	public float Drag = 10f;

	public override void Enter() {
		Player.PhysicsBody.SetSlideRate(Drag);
	}

	public override void Run() {
		if (Input.GetKeyDown(KeyCode.Space) && Player.PhysicsBody.IsGrounded()) StateMachine.Push<PlayerJumpingState>();
		
		base.Run();
	}

	public override void Exit() {
		Player.PhysicsBody.SetSlideRate(0);
	}

}