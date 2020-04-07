using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/DashingState")]
public class PlayerDashingState : PlayerState {

	public float DashPower = 10f;

	public float Cooldown = 3f;

	private float startTime;

	public override void Enter() {
		if (StateMachine.ShowDebugInfo) Debug.Log("Entered PDT");
		if (OffCooldown(Time.time)) Dash();
	}

	public override void Run() {
		if (Player.PhysicsBody.IsGrounded()) StateMachine.Pop();
	}

	private void Dash() {
		Player.PhysicsBody.ResetVelocity();
		Player.PhysicsBody.AddForce(Player.GetInput().normalized * DashPower, ForceMode.Impulse);
	}

	private bool OffCooldown(float currentTime) {
		startTime = startTime == 0 ? currentTime : 0;
		return startTime == currentTime || currentTime - startTime > Cooldown;
	}

}