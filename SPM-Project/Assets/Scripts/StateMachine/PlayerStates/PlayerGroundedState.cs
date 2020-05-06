using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public abstract class PlayerGroundedState : PlayerState {

	/// <summary>
	/// The duration of time after becoming ungrounded where jumping is still possible.
	/// </summary>
	public float JumpGracePeriod = 0.25f;

	private static float startTime = -1;

	public override void Enter() {
		Player.playerAnimator.SetBool("Falling", false);
		startTime = -1;
		ThrustersOff();
		base.Enter();
	}

	public override void Run() {
		if (!Player.PhysicsBody.IsGrounded() && startTime == -1) startTime = Time.time;
		if (!Player.PhysicsBody.IsGrounded() && Time.time - startTime > JumpGracePeriod) {
			startTime = -1;
			StateMachine.TransitionTo<PlayerFallingState>();
		}

		base.Run();
	}

	private void ThrustersOff() {
		Player.thrust1.SetActive(false);
		Player.thrust2.SetActive(false);
	}

}