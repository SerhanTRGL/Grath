using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordState_OnPlayer : SwordState{
    public override void EnterState(SwordStateMachine swordStateMachine){
        swordStateMachine.m_sword.transform.parent = swordStateMachine.m_sword.Holder.SwordHoldPoint;
        swordStateMachine.m_sword.transform.localPosition = Vector3.zero;
        swordStateMachine.m_sword.transform.localRotation = Quaternion.Euler(0,0,0);
        swordStateMachine.m_sword.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        swordStateMachine.m_sword.PlayerAnimator = swordStateMachine.m_sword.Holder.GetComponentInChildren<Animator>();
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
