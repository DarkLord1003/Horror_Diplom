using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateLanding : PlayerState
{
    public override StateType GetStateType()
    {
        return StateType.Landing;
    }

    public override void EnterState()
    {
        if (PlayerStateMachine == null)
            return;

        PlayerStateMachine.OnLand?.Invoke();
    }

    public override StateType UpdateState()
    {
        if (PlayerController == null || PlayerStateMachine == null)
            return StateType.None;

        if(PlayerController.MoveStatus == PlayerMoveStatus.NotMoving)
        {
            return StateType.Idle;
        }

        if(PlayerController.MoveStatus == PlayerMoveStatus.Walking)
        {
            return StateType.Walking;
        }

        return StateType.Landing;
    }

    public override void ExitState()
    {
        if (PlayerStateMachine == null)
            return;

        Debug.Log("Exit - Landing");
    }
}
