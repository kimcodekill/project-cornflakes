using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
[CreateAssetMenu(menuName = "PlayerState/MovingState")]
public class PlayerMovingState : PlayerGroundedState {

	public override void Enter() {
		Player.audioPlayerSteps.Play();
		Player.playerAnimator.SetBool("Walking", true);

		base.Enter();
	}

	public override void Run() {
		Vector3 input = Player.GetInput().normalized;
		if (input.magnitude == 0) StateMachine.TransitionTo<PlayerStandingState>();
		else Player.PhysicsBody.AddForce(input * Acceleration, ForceMode.Acceleration);

		Player.PhysicsBody.CapVelocity(TopSpeed);
		
		base.Run();
	}

	public override void Exit() {
		Player.audioPlayerSteps.Stop();
		Player.playerAnimator.SetBool("Walking", false);

		base.Exit();
	}

}