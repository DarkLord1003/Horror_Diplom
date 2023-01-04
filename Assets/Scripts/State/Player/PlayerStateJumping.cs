using UnityEngine;

public class PlayerStateJumping : PlayerState
{
    public override StateType GetStateType()
    {
        return StateType.Jumping;
    }

    public override void EnterState()
    {
        if (PlayerStateMachine == null)
            return;

        PlayerStateMachine.OnJump?.Invoke();
    }

    public override StateType UpdateState()
    {
        if (PlayerController == null)
            return StateType.None;

        if (PlayerController.MoveStatus == PlayerMoveStatus.Landing)
            return StateType.Landing;

        return StateType.Jumping;
    }

    public override void ExitState()
    {
        if (PlayerStateMachine == null)
            return;

        Debug.Log("Exit - Jumping");
    }
}
