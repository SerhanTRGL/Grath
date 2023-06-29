using UnityEngine;
public class PlayerStateMachine{
    private PlayerStatusWatcher _playerStatusWatcher;

    #region State Declarations
    public PlayerState currentState;
    public PlayerState_Dash dashState;
    public PlayerState_Dead deadState;
    public PlayerState_Idle idleState;
    public PlayerState_InAir inAirState;
    public PlayerState_Jump jumpState;
    public PlayerState_Running runningState;
    public PlayerState_SpinThrowSword spinThrowSwordState;
    #endregion
    
    public Player Player;
    public void StartStateMachine(Player player){
        _playerStatusWatcher = player._playerStatusWatcher;
        
        dashState = new PlayerState_Dash(_playerStatusWatcher);
        deadState = new PlayerState_Dead(_playerStatusWatcher);
        idleState = new PlayerState_Idle(_playerStatusWatcher);
        inAirState = new PlayerState_InAir(_playerStatusWatcher);
        jumpState = new PlayerState_Jump(_playerStatusWatcher);
        runningState = new PlayerState_Running(_playerStatusWatcher);
        spinThrowSwordState = new PlayerState_SpinThrowSword(_playerStatusWatcher);

        this.Player = player;
        this.currentState = idleState;
        currentState.EnterState(this);
    }

    public void ExecuteCurrentState() {
        currentState.ExecuteState(this);
    }

    public void SwitchState(PlayerState newState){
        currentState.ExitState(this);
        currentState = newState;
        newState.EnterState(this);
    }

}
