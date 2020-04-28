using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAirState : PlayerState {

	public float AirAcceleration = 10f;

	public float TopAirSpeed = 5f;

	private const float recheckTimeTreshold = 0.1f;

	private static float startAirTime = -1;

	protected bool skipEnter = false;

	public override void Enter() {
		if (startAirTime == -1) startAirTime = Time.time;

		base.Enter();
	}

	public override void Run() {
		if (Player.PhysicsBody.IsGrounded() && Time.time - startAirTime > recheckTimeTreshold) {
			jumpCount = 0;
			dashCount = 0;
			startAirTime = -1;
			StateMachine.Pop(skipEnter);
		}
		else Player.PhysicsBody.AddForce(Player.Input.horizontal * AirAcceleration, ForceMode.Acceleration);

		Player.PhysicsBody.CapHorizontalVelocity(TopAirSpeed);

		base.Run();
	}

}