using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState {

	public float AirAcceleration = 10f;

	public float TopAirSpeed = 5f;

	private const float recheckTimeTreshold = 0.1f;

	private float startAirTime;

	public override void Enter() {
		startAirTime = Time.time;
	}

	public override void Run() {
		if (Player.PhysicsBody.IsGrounded() && Time.time - startAirTime > recheckTimeTreshold) StateMachine.Pop();
		else Player.PhysicsBody.AddForce(Player.GetInput() * AirAcceleration, ForceMode.Acceleration);

		Player.PhysicsBody.CapHorizontalVelocity(TopAirSpeed);

		base.Run();
	}

}