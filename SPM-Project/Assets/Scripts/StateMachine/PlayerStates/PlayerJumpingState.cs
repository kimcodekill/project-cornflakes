using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/JumpingState")]
public class PlayerJumpingState : PlayerAirState {

	public float JumpHeight = 6f;

	public override void Enter() {
		if (StateMachine.ShowDebugInfo) Debug.Log("Entered PJT");
		Player.PhysicsBody.AddForce(Vector3.up * JumpHeight, ForceMode.Impulse);
		
		base.Enter();
	}

}