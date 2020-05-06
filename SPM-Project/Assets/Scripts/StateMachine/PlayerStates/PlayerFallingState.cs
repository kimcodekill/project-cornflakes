using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/FallingState")]
public class PlayerFallingState : PlayerAirState {

	public override void Enter() {
		DebugManager.UpdateRow("PlayerSTM" + Player.gameObject.GetInstanceID(), GetType().ToString());
		Player.playerAnimator.SetBool("Falling", true);
		if (jumpCount == 0) jumpCount++;

		base.Enter();
	}

}