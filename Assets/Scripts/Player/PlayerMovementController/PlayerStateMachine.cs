using System.Collections;
using System.Collections.Generic;
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
    public PlayerState_SpinThrowSword spinThrowSwordState = new PlayerState_SpinThrowSword();
    #endregion
    
    public Player m_player;
    public void StartStateMachine(Player player){
        Debug.Log("Player state machine started!");
        m_player = player;
        currentState = idleState;
        currentState.EnterState(this);
    }

    public void ExecuteCurrentState() {
        currentState.ExecuteState(this);
    }

    public void SwitchState(PlayerState state){
        currentState.ExitState(this);
        currentState = state;
        state.EnterState(this);
    }

}
