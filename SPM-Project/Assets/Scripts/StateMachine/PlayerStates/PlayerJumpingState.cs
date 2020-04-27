using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/JumpingState")]
public class PlayerJumpingState : PlayerAirState {

	public float JumpHeight = 6f;

	public override void Enter() {
		DebugManager.UpdateRow("PlayerSTM" + Player.gameObject.GetInstanceID(), GetType().ToString());

		if (Input.GetKey(KeyCode.Space) && jumpCount < 2) {
			Player.PhysicsBody.ResetVerticalSpeed();
			Player.PhysicsBody.AddForce(Vector3.up * JumpHeight, ForceMode.Impulse);
			jumpCount++;
		}
		
		base.Enter();
	}

	public override void Run() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			StateMachine.Push<PlayerJumpingState>();
		}
		
		base.Run();
	}

}