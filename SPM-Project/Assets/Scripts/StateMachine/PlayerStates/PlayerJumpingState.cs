using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Co-Author Joakim Linna

[CreateAssetMenu(menuName = "PlayerState/JumpingState")]
public class PlayerJumpingState : PlayerAirState {

	public float JumpHeight = 6f;

	public float JumpCooldown = 0.05f;

	private float startTime = -1;

	public override void Enter() {
		DebugManager.UpdateRow("PlayerSTM" + Player.gameObject.GetInstanceID(), GetType().ToString());
		
		startTime = Time.time;
		if (Player.GetInput().magnitude > 0f) Player.PhysicsBody.ChangeVelocityDirection(Player.GetInput().normalized);
		Player.PhysicsBody.SetAxisVelocity('y', 0f);
		Player.PhysicsBody.AddForce(Vector3.up * JumpHeight, ForceMode.Impulse);

		//I mean we could probably just tell the playeranim to do stuff from here but idk
		EventSystem.Current.FireEvent(new PlayerJumpEvent() { Description = "Player Jumped" });

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