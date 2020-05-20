using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
[CreateAssetMenu(menuName = "PlayerState/StandingState")]
public class PlayerStandingState : PlayerGroundedState {

	public override void Enter() {
		try { DebugManager.AddSection("PlayerSTM" + Player.gameObject.GetInstanceID(), ""); } catch (System.ArgumentException) { }
		DebugManager.UpdateRow("PlayerSTM" + Player.gameObject.GetInstanceID(), GetType().ToString());
		Player.PhysicsBody.UseStationaryPhysicsMaterial(true);

		base.Enter();
	}

	public override void Run() {
		if (Player.GetInput().magnitude > 0) StateMachine.TransitionTo<PlayerMovingState>();
		
		base.Run();
	}

	public override void Exit() {
		Player.PhysicsBody.UseStationaryPhysicsMaterial(false);

		base.Exit();
	}

}