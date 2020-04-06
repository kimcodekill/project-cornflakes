using UnityEngine;

public abstract class State : ScriptableObject {
    
    public object Owner;

    public StateMachine StateMachine;

    public virtual void Run() { }

    public virtual void Enter() { }

    public virtual void Exit() { }

}