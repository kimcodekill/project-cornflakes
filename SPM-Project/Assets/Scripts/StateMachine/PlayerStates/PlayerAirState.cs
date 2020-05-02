using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAirState : PlayerState {

	private const float recheckTimeTreshold = 0.1f;

	private static float startTime = -1;

	public override void Enter() {
		if (startTime == -1) startTime = Time.time;

		base.Enter();
	}

	public override void Run() {
		if (Player.PhysicsBody.IsGrounded() && Time.time - startTime > recheckTimeTreshold) {
			jumpCount = 0;
			dashCount = 0;
			startTime = -1;
			Player.PlayAudioMain(4, 1);
			StateMachine.TransitionTo<PlayerStandingState>();
		}
		else Player.PhysicsBody.AddForce(Player.GetInput().normalized * Acceleration, ForceMode.Acceleration);

		Player.PhysicsBody.CapHorizontalVelocity(TopSpeed);

		base.Run();
	}

}