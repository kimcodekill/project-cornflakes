using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : State {
	
	private PlayerController player;
	
	public PlayerController Player => player = player != null ? player : (PlayerController) Owner;

	protected int jumpCount = 0;
	protected int dashCount = 0;

	public override void Run() {
		if (dashCount < 1 && Input.GetKeyDown(KeyCode.LeftShift)) {
			dashCount++;
			StateMachine.Push<PlayerDashingState>();
		}
	}
}