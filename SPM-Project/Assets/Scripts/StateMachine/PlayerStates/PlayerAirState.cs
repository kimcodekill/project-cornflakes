using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState {

	private PhysicsBody physicsBody;

	public override void Enter() {
		physicsBody = Player.gameObject.GetComponent<PhysicsBody>();
	}

	public override void Run() {
		if (physicsBody.IsGrounded()) StateMachine.Pop();
	}

}