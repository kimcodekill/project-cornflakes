using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState {

	private PhysicsBody physicsBody;

	private const float recheckTimeTreshold = 0.1f;

	private float startAirTime;

	public override void Enter() {
		physicsBody = Player.gameObject.GetComponent<PhysicsBody>();
		startAirTime = Time.time;
	}

	public override void Run() {
		if (physicsBody.IsGrounded() && Time.time - startAirTime > recheckTimeTreshold ) StateMachine.Pop();
		
		base.Run();
	}

}