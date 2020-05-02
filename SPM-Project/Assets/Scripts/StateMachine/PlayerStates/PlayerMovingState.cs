using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/MovingState")]
public class PlayerMovingState : PlayerGroundedState {

	private double stepStartTime;
	private double stepEndTime;
	private double loops = 0;
	private bool playSteps;

	public override void Enter() {
		DebugManager.UpdateRow("PlayerSTM" + Player.gameObject.GetInstanceID(), GetType().ToString());
		loops = 0;
		if (AudioSettings.dspTime >= stepEndTime) { StartStepping(); /*Debug.Log("Started steppping from Enter");*/ }

		base.Enter();
	}

	public override void Run() {
		Vector3 input = Player.GetInput().normalized;
		if (input.magnitude == 0) StateMachine.TransitionTo<PlayerStandingState>();
		else Player.PhysicsBody.AddForce(input * Acceleration, ForceMode.Acceleration);

		Player.PhysicsBody.CapVelocity(TopSpeed);

		if (playSteps == true) loops += Time.deltaTime;
		if (AudioSettings.dspTime > stepEndTime && playSteps == false) { StartStepping(); /*Debug.Log("Started steppping from Run");*/ }

		base.Run();
	}

	private void StartStepping() {
		
		stepStartTime = AudioSettings.dspTime;
		Player.audioPlayerSteps.PlayScheduled(stepStartTime);
		playSteps = true;
	}

	public override void Exit() {
		//Debug.Log("playSteps was: " + playSteps);
		if (playSteps == true) {
			stepEndTime = stepStartTime + (Math.Floor(loops / Player.audioPlayerSteps.clip.length) + 1) * (Player.audioPlayerSteps.clip.length);
			Player.audioPlayerSteps.SetScheduledEndTime(stepEndTime);
			playSteps = false;
		}

		base.Exit();
	}

}