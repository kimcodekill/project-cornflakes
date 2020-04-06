using System;
using System.Collections.Generic;

public class StateMachine {

	private Stack<Type> stateStack = new Stack<Type>();

	private Dictionary<Type, State> states = new Dictionary<Type, State>();

	public StateMachine(object controller, State[] states) {
		for (int i = 0; i < states.Length; i++) {
			State instance = UnityEngine.Object.Instantiate(states[i]);
			instance.Owner = controller;
			instance.StateMachine = this;
			this.states.Add(instance.GetType(), instance);
			if (stateStack.Count == 0) stateStack.Push(instance.GetType());
		}
		if (stateStack.Count > 0) this.states[stateStack.Peek()].Enter();
	}

	public void Run() {
		states[stateStack.Peek()].Run();
	}

	public void Push<T>() where T : State {
		stateStack.Push(typeof(T));
		states[stateStack.Peek()].Enter();
	}

	public void Pop() {
		if (stateStack.Count > 0) states[stateStack.Pop()].Exit();
	}

	public void TransitionTo<T>() where T : State {
		Pop();
		Push<T>();
	}

}