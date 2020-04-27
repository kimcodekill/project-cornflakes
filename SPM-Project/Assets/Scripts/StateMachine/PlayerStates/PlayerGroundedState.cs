using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerGroundedState : PlayerState {

	public float Drag = 10f;

	public float AirDrag = 5f;

	public override void Enter() {
		Player.PhysicsBody.SetSlideRate(Drag);
		jumpCount = 0;
		dashCount = 0;

		base.Enter();
	}

	public override void Run() {
		//if (Input.GetKeyDown(KeyCode.Space) && Player.PhysicsBody.IsGrounded()) StateMachine.Push<PlayerJumpingState>();
		if (!Player.PhysicsBody.IsGrounded()) StateMachine.Push<PlayerFallingState>();

		base.Run();
	}

	public override void Exit() {
		Player.PhysicsBody.SetSlideRate(AirDrag);
	}

}