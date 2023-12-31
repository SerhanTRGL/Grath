using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordStateMachine{
   #region State Declarations
    public SwordState currentState;
    public SwordState_Attack1 attack1State = new SwordState_Attack1();
    public SwordState_Attack2 attack2State = new SwordState_Attack2();
    public SwordState_OnGround onGroundState = new SwordState_OnGround();
    public SwordState_OnPlayer onPlayerState = new SwordState_OnPlayer();
    public SwordState_SpinThrow spinThrowState = new SwordState_SpinThrow();
    #endregion

    public Sword m_sword;
    
    public void StartStateMachine(Sword sword){
        currentState = onGroundState;
        m_sword = sword;
        currentState.EnterState(this);
    }

    public void ExecuteCurrentState() {
        currentState.ExecuteState(this);
    }

    public void SwitchState(SwordState state){
        currentState.ExitState(this);
        currentState = state;
        state.EnterState(this);
    }
}
