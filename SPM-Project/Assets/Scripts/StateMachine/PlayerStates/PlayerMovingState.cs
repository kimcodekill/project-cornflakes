using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerState/MovingState")]
public class PlayerMovingState : PlayerGroundedState {

	[SerializeField] private float moveSpeed;

	public override void Enter() {
		Debug.Log("Entered PMT");
	}

	public override void Run() {
		Vector3 input = Player.GetInput();
		if (input.magnitude == 0) StateMachine.TransitionTo<PlayerStandingState>();
		else Player.transform.position += input * Time.deltaTime;
	}

}