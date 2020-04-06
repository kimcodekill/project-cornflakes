using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateMachine {

	private State currentState;

	private Dictionary<Type, State> states = new Dictionary<Type, State>();

	public StateMachine(object controller, State[] states) {
		for (int i = 0; i < states.Length; i++) {
			State instance = UnityEngine.Object.Instantiate(states[i]);
			instance.owner = controller;
			instance.stateMachine = this;

			this.states.Add(instance.GetType(), instance);
			currentState = currentState != null ? currentState : instance;
		}
		if (currentState != null) currentState.Enter();
	}

	public void Run() {
		currentState.Run();
	}

	public void TransitionTo<T>() where T : State {
		if (currentState != null) currentState.Exit();
		currentState = states[typeof(T)];
		currentState.Enter();
	}

}