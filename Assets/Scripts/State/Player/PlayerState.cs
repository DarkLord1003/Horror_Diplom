
public abstract class PlayerState : State
{
    protected PlayerController PlayerController;
    protected PlayerStateMachine PlayerStateMachine;

    public override void SetStateMachine(StateMachine stateMachine)
    {
        if(stateMachine.GetType() == typeof(PlayerStateMachine))
        {
            PlayerStateMachine = (PlayerStateMachine)stateMachine;
        }
    }

    public void SetPlayerConroller(PlayerController playerController)
    {
        PlayerController = playerController;
    }
}
