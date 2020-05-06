using UnityEngine;

//Author: Viktor Dahlberg
public abstract class State : ScriptableObject {
    
	/// <summary>
	/// The owner of the state.
	/// </summary>
    public object Owner;

	/// <summary>
	/// The state machine utilizing the state.
	/// </summary>
    public StateMachine StateMachine;

	/// <summary>
	/// Called every time <c>StateMachine.Run()</c> is called, usually every frame.
	/// </summary>
	public virtual void Run() { }

	/// <summary>
	/// Called when the state becomes the active state of the state machine.
	/// </summary>
    public virtual void Enter() { }

	/// <summary>
	/// Called when the state stops being the active state of the state machine.
	/// </summary>
    public virtual void Exit() { }

	/// <summary>
	/// Called when some state wishes to know if this state currently permits entry.
	/// </summary>
	/// <returns>Whether or not the state permits entry, returns <c>true</c> unless overridden by the state.</returns>
	public virtual bool CanEnter() { return true; }

	/// <summary>
	/// Some variable that can be sent with a <c>Push()</c>
	/// </summary>
	public object param;

}