using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordStateMachine{
   #region State Declarations
    public SwordState currentState;
    public SwordState_Attack1 attack1State = new SwordState_Attack1();
    public SwordState_Attack2 attack2State = new SwordState_Attack2();
    public SwordState_BossThrowSword bossThrowSwordState = new SwordState_BossThrowSword();
    public SwordState_OnBoss onBossState = new SwordState_OnBoss();
    public SwordState_OnGround onGroundState = new SwordState_OnGround();
    public SwordState_OnPlayer onPlayerState = new SwordState_OnPlayer();
    public SwordState_SpinThrow spinThrowState = new SwordState_SpinThrow();
    #endregion

    public Sword m_sword;
    
    public void StartStateMachine(Sword sword){
        if(sword.IsOnGround){
            currentState = onGroundState;
        }
        
        if(sword.IsOwnedByPlayer){
            currentState = onPlayerState;
        }

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
