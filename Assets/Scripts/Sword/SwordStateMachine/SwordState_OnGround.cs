using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordState_OnGround : SwordState{
    public override void EnterState(SwordStateMachine swordStateMachine){
        
    }

    public override void ExitState(SwordStateMachine swordStateMachine){
        
    }

    protected override void HandleStateLogic(SwordStateMachine swordStateMachine){
        
    }

    protected override void HandleStateSwitchLogic(SwordStateMachine swordStateMachine){
        //Cast a moderately sized box that if player is in it, they can pick the weapon.
        RaycastHit2D hit = Physics2D.BoxCast(
                                            origin: swordStateMachine.m_sword.transform.position,
                                            size: new Vector2(5, 5),
                                            angle: 0,
                                            direction: Vector2.zero);
        if(hit.collider.tag == "Player" && Input.GetKeyDown(KeyCode.E)){
            swordStateMachine.m_sword.transform.parent = hit.collider.transform;
            swordStateMachine.m_sword.transform.position = hit.collider.GetComponent<Player>().SwordMountPoint.position;
            swordStateMachine.m_sword.transform.rotation = hit.collider.GetComponent<Player>().SwordMountPoint.rotation;
            swordStateMachine.SwitchState(swordStateMachine.onPlayerState);
        }
    }
}
