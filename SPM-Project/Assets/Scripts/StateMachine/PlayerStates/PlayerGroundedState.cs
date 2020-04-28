using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerGroundedState : PlayerState {

	private const float recheckTimeTreshold = 0.1f;

	private static float startTime = -1;

	public override void Enter() {
		if (startTime == -1) startTime = Time.time;

		base.Enter();
	}

	public override void Run() {
		if (!Player.PhysicsBody.IsGrounded() && Time.time - startTime > recheckTimeTreshold) {
			startTime = -1;
			StateMachine.TransitionTo<PlayerFallingState>();
		}

		base.Run();
	}

}