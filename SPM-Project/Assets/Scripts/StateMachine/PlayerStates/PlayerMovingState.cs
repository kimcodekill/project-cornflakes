﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/MovingState")]
public class PlayerMovingState : PlayerGroundedState {

	public override void Enter() {
		if (StateMachine.ShowDebugInfo) Debug.Log("Entered PMS");
	}

	public override void Run() {
		Vector3 input = Player.GetInput();
		if (input.magnitude == 0) StateMachine.TransitionTo<PlayerStandingState>();
		else Player.PhysicsBody.AddForce(input);
		
		base.Run();
	}

}