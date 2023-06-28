using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordState_Attack1 : SwordState{
    private ComboManager m_comboManager;
    public override void EnterState(SwordStateMachine swordStateMachine){
        swordStateMachine.m_sword.PlayerAnimator.SetTrigger("FirstAttack");
        m_comboManager = swordStateMachine.m_sword.ComboManager;
        m_comboManager.ComboTimer = m_comboManager.ComboDuration;
    }

    public override void ExitState(SwordStateMachine swordStateMachine){
        
    }

    protected override void HandleStateLogic(SwordStateMachine swordStateMachine){
        swordStateMachine.m_sword.PlayerAnimator.SetFloat("ComboTimer", m_comboManager.ComboTimer);
    }

    protected override void HandleStateSwitchLogic(SwordStateMachine swordStateMachine){
        if(m_comboManager.ComboTimer > 0 && Input.GetMouseButtonDown(0)){
            swordStateMachine.SwitchState(swordStateMachine.attack2State);
        }
        if(m_comboManager.ComboTimer <= 0){
            swordStateMachine.SwitchState(swordStateMachine.onPlayerState);
        }
    }
}
