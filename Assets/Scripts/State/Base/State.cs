using UnityEngine;

public abstract class State
{
    protected StateMachine StateMachine;
    public virtual void SetStateMachine(StateMachine stateMachine) => StateMachine = stateMachine;
    public abstract StateType GetStateType();
    public abstract void EnterState();
    public abstract StateType UpdateState();
    public abstract void ExitState();
}
