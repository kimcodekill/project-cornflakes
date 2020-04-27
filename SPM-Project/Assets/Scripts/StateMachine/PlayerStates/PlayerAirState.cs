using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAirState : PlayerState {

	public float AirAcceleration = 10f;

	public float TopAirSpeed = 5f;

	private const float recheckTimeTreshold = 0.1f;

	private float startAirTime;

	protected bool skipEnter = false;

	public override void Enter() {
		skipEnter = false;
		startAirTime = Time.time;

		base.Enter();
	}

	public override void Run() {
		if (Player.PhysicsBody.IsGrounded() && Time.time - startAirTime > recheckTimeTreshold) {
			jumpCount = 0;
			dashCount = 0;
			StateMachine.Pop(skipEnter);
		}
		else Player.PhysicsBody.AddForce(Player.GetInput() * AirAcceleration, ForceMode.Acceleration);

		Player.PhysicsBody.CapHorizontalVelocity(TopAirSpeed);

		base.Run();
	}

}