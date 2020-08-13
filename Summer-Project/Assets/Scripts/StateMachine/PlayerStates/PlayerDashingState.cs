﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
[CreateAssetMenu(menuName = "PlayerState/DashingState")]
public class PlayerDashingState : PlayerState {

	public float DashSpeed;

	public float DashDuration;

	/// <summary>
	/// The amount of allowable altitude gain from a dash before we stop ascending.
	/// </summary>
	public float MaxYGain;

	/// <summary>
	/// If the dot product of <c>Vector3.down</c> and the current surface normal goes beyond this value, we abort the dash.
	/// </summary>
	public float StopDotTreshold;

	private float currentDashTime = 0;

	private float initialY;

	private bool dashed = false;

	private Charge charge;

	public override void Enter() {
		Player.dash1.SetActive(true);
		Player.dash2.SetActive(true);
        Player.dash3.SetActive(true);
        Player.playerAnimator.SetTrigger("Dashing");
		
		charge = charge == null ? Player.GetComponent<Charge>() : charge;
		
		Dash();

		base.Enter();
	}

	public override void Run() {
		if (dashed) {
			//We want to make sure the player cant fly up by dashing,
			//but we still want them to be able to dash uphill,
			//so if they are grounded when the Y axis delta exceeds the limit we set a new limit from that point on
			if (Player.transform.position.y - initialY > MaxYGain) {
				if (!Player.PhysicsBody.IsGrounded()) Player.PhysicsBody.SetAxisVelocity('y', 0);
				else initialY = Player.transform.position.y;
			}
			currentDashTime += Time.deltaTime;
			if (currentDashTime > DashDuration || Vector3.Dot(Vector3.down, Player.PhysicsBody.GetCurrentSurfaceNormal()) > StopDotTreshold) StateMachine.TransitionTo<PlayerFallingState>();
		}
		
		base.Run();
	}

	public override void Exit() {
		Player.dash1.SetActive(false);
		Player.dash2.SetActive(false);
        Player.dash3.SetActive(false);

        Player.PhysicsBody.SetAxisVelocity('y', 0f);
		Player.PhysicsBody.SetGravityEnabled(true);
		dashed = false;
		currentDashTime = 0f;

		base.Exit();
	}

	public override bool CanEnter() {
		return !dashed && (charge == null || charge.IsReady());
	}

	private void Dash() {
		initialY = Player.transform.position.y;
		if (charge != null) charge.Trigger();
		Player.PhysicsBody.ResetVelocity();
		
		Vector3 input = Player.GetInput();
		Vector3 impulse = (input.magnitude != 0 ? input.normalized : Vector3.ProjectOnPlane(Camera.main.transform.rotation * Vector3.forward, Vector3.up).normalized) * DashSpeed;
		impulse.y = 0;
		Player.PhysicsBody.AddForce(impulse, ForceMode.Impulse);
		Player.PhysicsBody.SetGravityEnabled(false);
		Player.PhysicsBody.SetAxisVelocity('y', 0f);
		
		dashed = true;
		Player.PlayAudioMain(2, 1);
	}

}