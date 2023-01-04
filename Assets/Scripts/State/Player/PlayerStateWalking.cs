using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateWalking : PlayerState
{
    public override StateType GetStateType()
    {
        return StateType.Walking;
    }

    public override void EnterState()
    {
        if (PlayerStateMachine == null)
            return;

        PlayerStateMachine.IsWalking = true;
    }

    public override StateType UpdateState()
    {
        PlayerStateMachine.VelocityX = PlayerController.InputVector.x;
        PlayerStateMachine.VelocityY = PlayerController.InputVector.y;

        if(PlayerController.MoveStatus == PlayerMoveStatus.NotMoving)
        {
            return StateType.Idle;
        }

        if(PlayerController.MoveStatus == PlayerMoveStatus.Sprinting)
        {
            return StateType.Sprinting;
        }

        if(PlayerController.MoveStatus == PlayerMoveStatus.Jumping)
        {
            return StateType.Jumping;
        }

        if(PlayerController.MoveStatus == PlayerMoveStatus.Crouching)
        {
            return StateType.Crouching;
        }

        return StateType.Walking;

    }

    public override void ExitState()
    {
        if (PlayerStateMachine == null)
            return;

        Debug.Log("Exit - Walking");

        PlayerStateMachine.IsWalking = false;
    }
}
