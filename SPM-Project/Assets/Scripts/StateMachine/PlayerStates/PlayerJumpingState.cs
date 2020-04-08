using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/JumpingState")]
public class PlayerJumpingState : PlayerAirState {
	[SerializeField] private float jumpHeight;
	public override void Enter() {
		if (StateMachine.ShowDebugInfo) Debug.Log("Entered PJS");
		Player.PhysicsBody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
		
		base.Enter();
	}

}