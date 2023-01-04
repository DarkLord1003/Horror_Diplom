using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStateController
{
    private List<State> _playerStates = new List<State>
    {
        new PlayerStateIdle(),
        new PlayerStateWalking(),
        new PlayerStateSprinting(),
        new PlayerStateJumping(),
        new PlayerStateCrouch(),
        new PlayerStateLanding()
    };

    public void InitStates(PlayerStateMachine stateMachine,PlayerController playerController)
    {
        if (stateMachine == null || playerController == null)
            return;

        foreach(PlayerState state in _playerStates)
        {
            state.SetStateMachine(stateMachine);
            state.SetPlayerConroller(playerController);

            stateMachine.AddState(state);
        }
    }
}
