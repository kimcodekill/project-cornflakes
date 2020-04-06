using System;
using System.Collections.Generic;

public class StateMachine {

	/// <summary>
	/// Toggles whether or not state debug info should be shown.
	/// </summary>
	public bool ShowDebugInfo = false;

	private Stack<Type> stateStack = new Stack<Type>();

	private Dictionary<Type, State> states = new Dictionary<Type, State>();

	/// <summary>
	/// Creates a new pushdown automata state machine and begins running the state on index 0 in the passed array.
	/// </summary>
	/// <param name="controller">The host of the state machine.</param>
	/// <param name="states">The different kinds of states the state machine can utilize.</param>
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

	/// <summary>
	/// Runs the current state.
	/// </summary>
	public void Run() {
		states[stateStack.Peek()].Run();
	}

	/// <summary>
	/// Adds an element to the state stack and enters it, calling <c>Enter()</c> on it in the process.
	/// </summary>
	/// <typeparam name="T">The type of the state to enter.</typeparam>
	public void Push<T>() where T : State {
		stateStack.Push(typeof(T));
		states[stateStack.Peek()].Enter();
	}

	/// <summary>
	/// Removes the topmost element of the state stack, calling <c>Exit()</c> on the removed state in the process.
	/// If any elements remain in the stack, that state is entered, <c>Enter()</c> is called on the topmost state.
	/// </summary>
	/// <exception cref="System.InvalidOperationException">Thrown if a <c>Pop()</c> is attempted even if <c>Push&lt;T&gt;()</c> hasn't been used to add a state to the state stack.</exception>
	public void Pop() {
		if (stateStack.Count <= 1) throw new InvalidOperationException("No state to return to, use TransitionTo<T>() instead.");
		else {
			if (stateStack.Count > 0) states[stateStack.Pop()].Exit();
			if (stateStack.Count > 0) states[stateStack.Peek()].Enter();
		}
	}

	/// <summary>
	/// Enter a new state, calling <c>Exit()</c> on the old state and <c>Enter()</c> on the new state in the process.
	/// </summary>
	/// <typeparam name="T">The type of the state to enter.</typeparam>
	public void TransitionTo<T>() where T : State {
		Pop();
		Push<T>();
	}

}