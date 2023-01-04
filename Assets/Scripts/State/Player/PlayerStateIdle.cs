using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle : PlayerState
{
    public override StateType GetStateType()
    {
        return StateType.Idle;
    }

    public override void EnterState()
    {
        if (PlayerStateMachine == null)
            return;

        PlayerStateMachine.VelocityX = 0f;
        PlayerStateMachine.VelocityY = 0f;
        PlayerStateMachine.IsWalking = false;
        PlayerStateMachine.IsSprinting = false;

    }

    public override StateType UpdateState()
    {
        if (PlayerController == null || PlayerStateMachine == null)
            return StateType.None;

        if(PlayerController.MoveStatus == PlayerMoveStatus.Walking)
        {
            return StateType.Walking;
        }

        if(PlayerController.MoveStatus == PlayerMoveStatus.Jumping)
        {
            return StateType.Jumping;
        }

        if(PlayerController.MoveStatus == PlayerMoveStatus.Crouching)
        {
            return StateType.Crouching;
        }

        return StateType.Idle;
    }

    public override void ExitState()
    {
        Debug.Log("Exit - Idle");
    }
}
