﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAirState : PlayerState {

	private const float recheckTimeTreshold = 0.1f;

	private static float startTime = -1;

	public override void Enter() {
		if (startTime == -1 && !Player.PhysicsBody.IsGrounded()) startTime = Time.time;

		base.Enter();
	}

	public override void Run() {
		if (Player.PhysicsBody.IsGrounded() && Time.time - startTime > recheckTimeTreshold) {
			jumpCount = 0;
			dashCount = 0;
			startTime = -1;
			StateMachine.TransitionTo<PlayerMovingState>();
		}
		else Player.PhysicsBody.AddForce(Player.Input.directional * Acceleration, ForceMode.Acceleration);

		Player.PhysicsBody.CapHorizontalVelocity(TopSpeed);

		base.Run();
	}

}