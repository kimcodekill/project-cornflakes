using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/MovingState")]
public class PlayerMovingState : PlayerGroundedState {

	public override void Enter() {
		DebugManager.UpdateRow("PlayerSTM" + Player.gameObject.GetInstanceID(), GetType().ToString());

		base.Enter();
	}

	public override void Run() {
		Vector3 input = Player.GetInput().normalized;
		if (input.magnitude == 0) StateMachine.TransitionTo<PlayerStandingState>();
		else Player.PhysicsBody.AddForce(input * Acceleration, ForceMode.Acceleration);

		Player.PhysicsBody.CapVelocity(TopSpeed);

		base.Run();
	}

}