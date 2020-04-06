using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateMachine {

	private Stack<Type> stateStack = new Stack<Type>();

	private Dictionary<Type, State> states = new Dictionary<Type, State>();

	public StateMachine(object controller, State[] states) {
		for (int i = 0; i < states.Length; i++) {
			State instance = UnityEngine.Object.Instantiate(states[i]);
			instance.owner = controller;
			instance.stateMachine = this;
			this.states.Add(instance.GetType(), instance);
			if (stateStack.Peek() != null) stateStack.Push(instance.GetType());
		}
		if (stateStack.Peek() != null) this.states[stateStack.Peek()].Enter();
	}

	public void Run() {
		states[stateStack.Peek()].Run();
	}

	public void Push<T>() where T : State {
		stateStack.Push(typeof(T));
		states[stateStack.Peek()].Enter();
	}

	public void Pop() {
		if (stateStack.Peek() != null) states[stateStack.Pop()].Exit();
	}

	public void TransitionToState<T>() where T : State {
		Pop();
		Push<T>();
	}

}