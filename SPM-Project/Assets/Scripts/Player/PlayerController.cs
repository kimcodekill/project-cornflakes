using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private StateMachine stm;

	[SerializeField] private State[] states;

	private void Start() {
		stm = new StateMachine(this, states);
	}

}