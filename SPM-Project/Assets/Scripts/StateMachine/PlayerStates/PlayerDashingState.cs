using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/DashingState")]
public class PlayerDashingState : PlayerAirState {

	public float DashSpeed = 7f;

	public float Cooldown = 3f;

	public float DashDuration = 0.5f;

	private float startTime = -1;

	private float currentDashTime = 0;

	private bool dashed = false;

	private Afterburner afterburner;

	private bool warned;

	public override void Enter() {
		DebugManager.UpdateRow("PlayerSTM" + Player.gameObject.GetInstanceID(), GetType().ToString());
		
		afterburner = afterburner == null ? Player.GetComponent<Afterburner>() : afterburner;
		if (afterburner == null && !warned) {
			warned = true;
			Debug.LogWarning("No active Afterburner. Add an Afterburner component to your Player.");
		}
		if (Input.GetKey(KeyCode.LeftShift) && !dashed && OffCooldown(Time.time) && (afterburner == null || afterburner.CanFire()) && dashCount < 1) Dash();
		else StateMachine.Pop(true);
		
		base.Enter();
	}

	public override void Run() {
		if (dashed) currentDashTime += Time.deltaTime;
		if (dashed && currentDashTime > DashDuration) {
			StateMachine.Pop();
		}

		base.Run();
	}

	public override void Exit() {
		Player.PhysicsBody.SetGravityEnabled(true);
		dashed = false;
		currentDashTime = 0f;

		base.Exit();
	}

	private void Dash() {
		dashCount++;
		if (afterburner != null) afterburner.Fire();
		Player.PhysicsBody.ResetVelocity();
		Vector3 input = Player.GetInput();
		Vector3 impulse = (input.magnitude != 0 ? input.normalized : Vector3.ProjectOnPlane(Camera.main.transform.rotation * Vector3.forward, Vector3.up).normalized) * DashSpeed;
		impulse.y = 0;
		Player.PhysicsBody.AddForce(impulse, ForceMode.Impulse);
		Player.PhysicsBody.SetGravityEnabled(false);
		Player.PhysicsBody.ResetVerticalSpeed();
		dashed = true;
	}

	private bool OffCooldown(float currentTime) {
		if (startTime == -1 || currentTime - startTime > Cooldown) {
			startTime = currentTime;
			return true;
		}
		else return false;
	}

}