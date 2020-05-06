using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
[CreateAssetMenu(menuName = "PlayerState/MovingState")]
public class PlayerMovingState : PlayerGroundedState {

	private double stepStartTime;
	private double stepEndTime;
	private double loops = 0;
	private bool playSteps;

	public override void Enter() {
		DebugManager.UpdateRow("PlayerSTM" + Player.gameObject.GetInstanceID(), GetType().ToString());
		loops = 0;
		Player.audioPlayerSteps.Play();

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

		base.Exit();
	}

}