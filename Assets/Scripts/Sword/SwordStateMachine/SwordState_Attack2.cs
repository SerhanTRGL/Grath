using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordState_Attack2 : SwordState{
    private ComboManager m_comboManager;
    public override void EnterState(SwordStateMachine swordStateMachine){
        swordStateMachine.m_sword.PlayerAnimator.SetTrigger("SecondAttack");
        m_comboManager = swordStateMachine.m_sword.ComboManager;
        m_comboManager.ComboTimer = m_comboManager.ComboDuration;

        RaycastHit2D hit = Physics2D.Raycast(swordStateMachine.m_sword.Holder.transform.position + new Vector3(0,1), swordStateMachine.m_sword.Holder.transform.right, 2);
        if(hit.collider != null && hit.collider.tag == "BossPart"){
            hit.collider.GetComponent<BodyPartReference>().AssociatedBodyPart.GetComponent<IDamageable>().TakeDamage(swordStateMachine.m_sword.damage);
        }
    }

    public override void ExitState(SwordStateMachine swordStateMachine){
        
    }

    protected override void HandleStateLogic(SwordStateMachine swordStateMachine){
        swordStateMachine.m_sword.PlayerAnimator.SetFloat("ComboTimer", m_comboManager.ComboTimer);
        Debug.DrawRay(swordStateMachine.m_sword.Holder.transform.position + new Vector3(0,1), swordStateMachine.m_sword.Holder.transform.right, Color.red);
    }

    protected override void HandleStateSwitchLogic(SwordStateMachine swordStateMachine){
        if(m_comboManager.ComboTimer > 0 && Input.GetMouseButtonDown(0)){
            swordStateMachine.SwitchState(swordStateMachine.attack1State);
        }
        if(m_comboManager.ComboTimer <= 0){
            swordStateMachine.SwitchState(swordStateMachine.onPlayerState);
        }
    }
}
