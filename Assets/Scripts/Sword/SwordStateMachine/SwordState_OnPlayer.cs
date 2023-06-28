using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordState_OnPlayer : SwordState{
    public override void EnterState(SwordStateMachine swordStateMachine){
        swordStateMachine.m_sword.PlayerAnimator.SetBool("isOnPlayerBack", true);
    }

    public override void ExitState(SwordStateMachine swordStateMachine){
        swordStateMachine.m_sword.PlayerAnimator.SetBool("isOnPlayerBack", false);
    }

    protected override void HandleStateLogic(SwordStateMachine swordStateMachine){

    }

    protected override void HandleStateSwitchLogic(SwordStateMachine swordStateMachine){
        if(Input.GetMouseButtonDown(0)){
            swordStateMachine.SwitchState(swordStateMachine.attack1State);
        }
    }
}
