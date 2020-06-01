using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
[CreateAssetMenu(menuName = "PlayerState/FallingState")]
public class PlayerFallingState : PlayerAirState {

	public override void Enter() {
		Player.playerAnimator.SetBool("Falling", true);
		if (jumpCount == 0) jumpCount++;

		base.Enter();
	}
}