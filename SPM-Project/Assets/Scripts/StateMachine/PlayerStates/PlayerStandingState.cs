using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/StandingState")]
public class PlayerStandingState : PlayerGroundedState {

	public override void Enter() {
		Debug.Log("Entered PST");
	}

	public override void Run() {
		if (Player.GetInput().magnitude > 0) StateMachine.TransitionTo<PlayerMovingState>();
		
		base.Run();
	}

}