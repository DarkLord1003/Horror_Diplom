using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateCrouch : PlayerState
{
    public override StateType GetStateType()
    {
        return StateType.Crouching;
    }

    public override void EnterState()
    {
        if (PlayerStateMachine == null)
            return;

        PlayerStateMachine.IsCrouching = true;
    }

    public override StateType UpdateState()
    {
        if (PlayerStateMachine == null || PlayerController == null)
            return StateType.None;

        PlayerStateMachine.VelocityX = PlayerController.InputVector.x;
        PlayerStateMachine.VelocityY = PlayerController.InputVector.y;

        if (PlayerController.MoveStatus == PlayerMoveStatus.NotMoving)
        {
            return StateType.Idle;
        }

        if(PlayerController.MoveStatus == PlayerMoveStatus.Walking)
        {
            return StateType.Walking;
        }

        return StateType.Crouching;
    }

    public override void ExitState()
    {
        if (PlayerStateMachine == null)
            return;

        PlayerStateMachine.IsCrouching = false;
    }
}
