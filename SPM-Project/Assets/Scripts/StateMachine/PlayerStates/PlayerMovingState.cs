using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/MovingState")]
public class PlayerMovingState : PlayerGroundedState {

	public float Acceleration = 1f;

	public float TopSpeed = 5f;

	public override void Enter() {
		if (StateMachine.ShowDebugInfo) Debug.Log("Entered PMT");
	}

	public override void Run() {
		Vector3 input = Player.GetInput();
		if (input.magnitude == 0) StateMachine.TransitionTo<PlayerStandingState>();
		else Player.PhysicsBody.AddForce(input * Acceleration, ForceMode.Acceleration);

		Player.PhysicsBody.CapVelocity(TopSpeed);

		base.Run();
	}

}