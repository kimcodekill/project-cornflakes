﻿using System;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class StateMachine {

	private Stack<Type> stateStack = new Stack<Type>();

	private Dictionary<Type, State> states = new Dictionary<Type, State>();

	private State previousState;

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
	/// Adds an element to the state stack and enters it, calling <c>Enter()</c> on it in the process,
	/// and <c>Exit()</c> on the previous state, if exists.
	/// </summary>
	/// <typeparam name="T">The type of the state to enter.</typeparam>
	/// <param name="param">Used freely to transfer information between states.</param>
	public void Push<T>(object param = null) where T : State {
		if (stateStack.Count > 0) {
			previousState = states[stateStack.Peek()];
			states[stateStack.Peek()].Exit();
		}
		stateStack.Push(typeof(T));
		states[stateStack.Peek()].param = param;
		states[stateStack.Peek()].Enter();
	}

	/// <summary>
	/// Removes the topmost element of the state stack, calling <c>Exit()</c> on the removed state in the process.
	/// If any elements remain in the stack, that state is entered, <c>Enter()</c> is called on the topmost state.
	/// </summary>
	/// <param name="skipEnter">Whether or not the <c>Enter()</c> function of the switched to state should be called.</param>
	/// <exception cref="System.InvalidOperationException">Thrown if a <c>Pop()</c> is attempted even if <c>Push&lt;T&gt;()</c> hasn't been used to add a state to the state stack.</exception>
	public void Pop(bool skipEnter = false) {
		DoPop(false, skipEnter);
	}

	/// <summary>
	/// Checks if the previous state is equal to the type parameter.
	/// </summary>
	/// <typeparam name="T">The type to compare to.</typeparam>
	/// <returns>Whether or not the type is equal to the previous state type.</returns>
	public bool IsPreviousState<T>() {
		return previousState.GetType() == typeof(T);
	}

	/// <summary>
	/// Evaluates the states CanEnter() function to determine if transitioning is allowed.
	/// </summary>
	/// <typeparam name="T">The type of state to check.</typeparam>
	/// <returns></returns>
	public bool CanEnterState<T>() where T : State {
		return states[typeof(T)].CanEnter();
	}

	/// <summary>
	/// Returns the type of the current state.
	/// </summary>
	/// <returns>The type of the current state.</returns>
	public Type GetCurrentState() {
		return states[stateStack.Peek()].GetType();
	}

	private void DoPop(bool isInternal, bool skipEnter = false) {
		if (stateStack.Count <= 1 && !isInternal) throw new InvalidOperationException("No state to return to, use TransitionTo<T>() instead.");
		else {
			if (stateStack.Count > 0) states[stateStack.Pop()].Exit();
			if (stateStack.Count > 0 && !skipEnter) states[stateStack.Peek()].Enter();
		}
	}

	/// <summary>
	/// Enter a new state, calling <c>Exit()</c> on the old state and <c>Enter()</c> on the new state in the process.
	/// </summary>
	/// <typeparam name="T">The type of the state to enter.</typeparam>
	/// <param name="param">Used freely to transfer information between states.</param>
	public void TransitionTo<T>(object param = null) where T : State {
		previousState = states[stateStack.Peek()];
		DoPop(true);
		Push<T>(param);
	}

}