using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateSprinting : PlayerState
{
    public override StateType GetStateType()
    {
        return StateType.Sprinting;
    }

    public override void EnterState()
    {
        if (PlayerStateMachine == null)
            return;

        PlayerStateMachine.IsSprinting = true;
    }

    public override StateType UpdateState()
    {
        if (PlayerController == null)
            return StateType.None;

        PlayerStateMachine.VelocityX = PlayerController.InputVector.x;
        PlayerStateMachine.VelocityY = PlayerController.InputVector.y;

        if (PlayerController.MoveStatus == PlayerMoveStatus.Walking)
            return StateType.Walking;

        if(PlayerController.MoveStatus == PlayerMoveStatus.Jumping)
        {
            return StateType.Sprinting;
        }

        if(PlayerController.MoveStatus == PlayerMoveStatus.NotMoving)
        {
            return StateType.Idle;
        }

        return StateType.Sprinting;
    }

    public override void ExitState()
    {
        if (PlayerStateMachine == null)
            return;

        Debug.Log("Exit - Sprinting");

        PlayerStateMachine.IsSprinting = false;
    }
}
