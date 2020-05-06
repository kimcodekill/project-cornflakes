using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
[CreateAssetMenu(menuName = "PlayerState/DashingState")]
public class PlayerDashingState : PlayerState {

	public float DashSpeed;

	public float Cooldown;

	public float DashDuration;

	/// <summary>
	/// The amount of allowable altitude gain from a dash before we stop ascending.
	/// </summary>
	public float MaxYGain;

	/// <summary>
	/// If the dot product of <c>Vector3.down</c> and the current surface normal goes beyond this value, we abort the dash.
	/// </summary>
	public float StopDotTreshold;

	/// <summary>
	/// Toggles whether or not the player should be allowed to dash while grounded.
	/// </summary>
	public bool AllowGrounded;

	private float startTime = -1;

	private float currentDashTime = 0;

	private float initialY;

	private bool dashed = false;

	private Afterburner afterburner;

	private bool warned;

	public override void Enter() {
		Player.dash1.SetActive(true);
		Player.dash2.SetActive(true);
		Player.playerAnimator.SetTrigger("Dashing");
		DebugManager.UpdateRow("PlayerSTM" + Player.gameObject.GetInstanceID(), GetType().ToString());
		
		afterburner = afterburner == null ? Player.GetComponent<Afterburner>() : afterburner;
		if (afterburner == null && !warned) {
			warned = true;
			Debug.LogWarning("No active Afterburner. Add an Afterburner component to your Player.");
		}
		
		Dash();

		base.Enter();
	}

	public override void Run() {
		if (dashed) {
			if (Player.transform.position.y - initialY > MaxYGain) Player.PhysicsBody.SetAxisVelocity('y', 0);
			currentDashTime += Time.deltaTime;
			if (currentDashTime > DashDuration || Vector3.Dot(Vector3.down, Player.PhysicsBody.GetCurrentSurfaceNormal()) > StopDotTreshold) StateMachine.TransitionTo<PlayerFallingState>();
		}
		
		base.Run();
	}

	public override void Exit() {
		Player.dash1.SetActive(false);
		Player.dash2.SetActive(false);
		Player.PhysicsBody.SetGravityEnabled(true);
		dashed = false;
		currentDashTime = 0f;

		base.Exit();
	}

	public override bool CanEnter() {
		return !dashed && OffCooldown(Time.time) && (afterburner == null || afterburner.CanFire()) && dashCount < 1 && ((!AllowGrounded && !Player.PhysicsBody.IsGrounded()) || AllowGrounded);
	}

	private void Dash() {
		initialY = Player.transform.position.y;
		dashCount++;
		if (afterburner != null) afterburner.Fire();
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

	private bool OffCooldown(float currentTime) {
		if (startTime == -1 || currentTime - startTime > Cooldown) {
			startTime = currentTime;
			return true;
		}
		else return false;
	}

}