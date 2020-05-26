using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
[CreateAssetMenu(menuName = "PlayerState/SprintingState")]
public class PlayerSprintingState : PlayerGroundedState {

	public bool UseAfterburner;

	private Afterburner afterburner;

	public override void Enter() {
		afterburner = afterburner == null ? Player.GetComponent<Afterburner>() : afterburner;

		base.Enter();
	}

	public override void Run() {
		Vector3 input = Player.GetInput().normalized;
		if (input.magnitude == 0) StateMachine.TransitionTo<PlayerStandingState>();
		else if (!Input.GetKey(KeyCode.LeftShift) || (afterburner != null && UseAfterburner && !afterburner.CanFire())) StateMachine.TransitionTo<PlayerMovingState>();
		else {
			Player.PhysicsBody.AddForce(input * Acceleration, ForceMode.Acceleration);
			afterburner?.Fire();
		}

		Player.PhysicsBody.CapVelocity(TopSpeed);

		base.Run();
	}

}