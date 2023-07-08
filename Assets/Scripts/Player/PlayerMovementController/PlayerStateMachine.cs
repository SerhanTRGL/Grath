using UnityEngine;
public class PlayerStateMachine{
    #region State Declarations
    public PlayerState currentState;
    public PlayerState_Dash dashState = new PlayerState_Dash();
    public PlayerState_Dead deadState = new PlayerState_Dead();
    public PlayerState_Idle idleState = new PlayerState_Idle();
    public PlayerState_InAir inAirState = new PlayerState_InAir();
    public PlayerState_Jump jumpState = new PlayerState_Jump();
    public PlayerState_Running runningState = new PlayerState_Running();
    #endregion
    
    public Player Player;
    public bool hasJumped;

    public void StartStateMachine(Player player){
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
