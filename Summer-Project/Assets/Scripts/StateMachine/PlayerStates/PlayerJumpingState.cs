﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
[CreateAssetMenu(menuName = "PlayerState/JumpingState")]
public class PlayerJumpingState : PlayerAirState {

	public float JumpHeight = 6f;

	public float JumpCooldown = 0.05f;

	private float startTime = -1;

	public override void Enter() {
		startTime = Time.time;
		if (Player.GetInput().magnitude > 0f) Player.PhysicsBody.ChangeVelocityDirection(Player.GetInput().normalized);
		Player.PhysicsBody.SetAxisVelocity('y', 0f);
		Player.PhysicsBody.AddForce(Vector3.up * JumpHeight, ForceMode.Impulse);
		/*if (jumpCount == 0)*/ Player.PlayAudioMain(3, 1);
		//else if (jumpCount == 1) Player.PlayAudioMain(3, 1);		If we make another sound for the double jump.

		StateMachine.TransitionTo<PlayerFallingState>();
	}

	public override bool CanEnter() {
		return !(Time.time - startTime < JumpCooldown) && (jumpCount < 2 || startTime == -1);
	}

	public override void Exit() {
		jumpCount++;

		base.Exit();
	}

}