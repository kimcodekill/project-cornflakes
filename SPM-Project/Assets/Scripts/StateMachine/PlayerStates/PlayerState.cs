﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public abstract class PlayerState : State {

	public Effects UseEffects;

	public float Drag;
	public float Acceleration;
	public float TopSpeed;

	private PlayerController player;
	
	public PlayerController Player => player = player != null ? player : (PlayerController) Owner;

	protected static int jumpCount = 0;
	protected static int dashCount = 0;

	public override void Enter() {
		DebugManager.UpdateRow("PlayerSTM" + Player.gameObject.GetInstanceID(), StateMachine.GetCurrentState().ToString());
		Player.PhysicsBody.SetSlideRate(Drag);
		if (Player.PhysicsBody.IsGrounded()) dashCount = 0;
		ToggleEffects(UseEffects);
	}

	public override void Run() {
		if (Player.Input.doJump && StateMachine.CanEnterState<PlayerJumpingState>()) {
			StateMachine.TransitionTo<PlayerJumpingState>();
		}
		
	}

	private void ToggleEffects(Effects effects) {
		ToggleThrusters(effects.UseThrusters);
	}

	private void ToggleThrusters(bool enabled) {
		Player.thrust1.SetActive(enabled);
		Player.thrust2.SetActive(enabled);
	}

	[System.Serializable]
	public struct Effects {
		public bool UseThrusters;
	}
}