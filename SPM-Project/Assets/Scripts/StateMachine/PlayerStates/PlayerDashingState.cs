using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/DashingState")]
public class PlayerDashingState : PlayerState {

	public float DashPower = 7f;

	public float Cooldown = 3f;

	private float startTime = -1;

	public override void Enter() {
		if (StateMachine.ShowDebugInfo) Debug.Log("Entered PDT");
		if (OffCooldown(Time.time)) Dash();
	}

	public override void Run() {
		if (Player.PhysicsBody.IsGrounded()) StateMachine.Pop();
	}

	private void Dash() {
		Player.PhysicsBody.ResetVelocity();
		Vector3 input = Player.GetInput();
		if (input.magnitude == 0 && !Player.PhysicsBody.IsGrounded()) Player.PhysicsBody.AddForce(Vector3.up * DashPower, ForceMode.Impulse);
		else Player.PhysicsBody.AddForce(Vector3.up + input.normalized * DashPower, ForceMode.Impulse);
	}

	private bool OffCooldown(float currentTime) {
		if (startTime == -1 || currentTime - startTime > Cooldown) {
			startTime = currentTime;
			return true;
		}
		else return false;
	}

}