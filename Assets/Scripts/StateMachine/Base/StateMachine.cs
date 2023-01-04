using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public abstract class StateMachine : MonoBehaviour
{
    [Header("Current Active State")]
    [SerializeField] protected StateType CurrentStateType;

    protected Dictionary<StateType, State> States;
    protected State CurrentState;
    protected Animator Animator;

    private void Awake()
    {
        States = new Dictionary<StateType, State>();
        Animator = GetComponentInChildren<Animator>();
    }

    public virtual void Update()
    {
        if (CurrentState == null || CurrentStateType == StateType.None)
            return;

        StateType newStateType = CurrentState.UpdateState();
        if (newStateType != CurrentStateType)
        {
            State newState = null;
            if (States.TryGetValue(newStateType, out newState))
            {
                CurrentState.ExitState();
                newState.EnterState();
                CurrentState = newState;
            }
            else if (States.TryGetValue(StateType.Idle, out newState))
            {
                CurrentState.ExitState();
                newState.EnterState();
                CurrentState = newState;
            }
            else
            {
                CurrentStateType = StateType.None;
                CurrentState = null;
            }

            CurrentStateType = newStateType;
        }
    }

    public void AddState(State state)
    {
        if (state == null)
            return;

        if (!States.ContainsKey(state.GetStateType()))
        {
            States.Add(state.GetStateType(), state);
        }
    }
}
