using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/JumpingState")]
public class PlayerJumpingState : PlayerAirState {

	public override void Enter() {
		if (StateMachine.ShowDebugInfo) Debug.Log("Entered PJT");
		Player.PhysicsBody.AddForce(Vector3.up * 6, ForceMode.Impulse);
		
		base.Enter();
	}

}