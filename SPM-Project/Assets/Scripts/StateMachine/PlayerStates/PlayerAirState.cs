using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState {

	public override void Run() {
		Player.transform.position += Vector3.down * Time.deltaTime;
		if (Player.transform.position.y < 0) {
			Player.transform.position = new Vector3(Player.transform.position.x, 0, Player.transform.position.z);
			StateMachine.Pop();
		}
	}

}