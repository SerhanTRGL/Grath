public abstract class PlayerState{
    protected PlayerStatusWatcher _playerStatusWatcher;
    public PlayerState(PlayerStatusWatcher playerStatusWatcher){
        _playerStatusWatcher = playerStatusWatcher;
    }
    public abstract void EnterState(PlayerStateMachine playerStateMachine);
    public void ExecuteState(PlayerStateMachine playerStateMachine){
        HandleStateLogic(playerStateMachine);
        HandleStateSwitchLogic(playerStateMachine);
    }
    public abstract void ExitState(PlayerStateMachine playerStateMachine);
    protected abstract void HandleStateLogic(PlayerStateMachine playerStateMachine);
    protected abstract void HandleStateSwitchLogic(PlayerStateMachine playerStateMachine);
}
