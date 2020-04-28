using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/JumpingState")]
public class PlayerJumpingState : PlayerAirState {

	public float JumpHeight = 6f;

	public float JumpCooldown = 0.05f;

	private float startTime = -1;

	public override void Enter() {
		DebugManager.UpdateRow("PlayerSTM" + Player.gameObject.GetInstanceID(), GetType().ToString());
		
		if (Time.time - startTime < JumpCooldown) StateMachine.Pop();
		else if (param != null && jumpCount < 2 || startTime == -1) {
			startTime = Time.time;
			param = null;
			jumpCount++;
			Player.PhysicsBody.ChangeVelocityDirection(Player.Input.horizontal);
			Player.PhysicsBody.ResetVerticalSpeed();
			Player.PhysicsBody.AddForce(Vector3.up * JumpHeight, ForceMode.Impulse);
		}

		base.Enter();
	}

}