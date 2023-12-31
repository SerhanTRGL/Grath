using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SwordState_OnGround : SwordState{
    private TextMeshProUGUI pickUpText;
    public override void EnterState(SwordStateMachine swordStateMachine){
        pickUpText = swordStateMachine.m_sword.GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);
    }

    public override void ExitState(SwordStateMachine swordStateMachine){
        pickUpText.gameObject.SetActive(false);
    }

    protected override void HandleStateLogic(SwordStateMachine swordStateMachine){
        RaycastHit2D hit = Physics2D.BoxCast(
                                            origin: swordStateMachine.m_sword.transform.position,
                                            size: new Vector2(5, 5),
                                            angle: 0,
                                            direction: Vector2.zero,
                                            5,
                                            LayerMask.GetMask("PlayerTrigger")
                                            );

        if(hit.collider != null && hit.collider.tag == "Player"){
            pickUpText.gameObject.SetActive(true);
        }
        else{
            pickUpText.gameObject.SetActive(false);
        }
    }

    protected override void HandleStateSwitchLogic(SwordStateMachine swordStateMachine){
        //Cast a moderately sized box that if player is in it, they can pick the weapon.
        RaycastHit2D hit = Physics2D.BoxCast(
                                            origin: swordStateMachine.m_sword.transform.position,
                                            size: new Vector2(5, 5),
                                            angle: 0,
                                            direction: Vector2.zero,
                                            5,
                                            LayerMask.GetMask("PlayerTrigger")
                                            );
        if(hit.collider != null && hit.collider.tag == "Player" && Input.GetKeyDown(KeyCode.E)){
            hit.collider.GetComponentInParent<Player>().AcquireSword(swordStateMachine.m_sword);
            swordStateMachine.m_sword.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            swordStateMachine.m_sword.Holder = hit.collider.GetComponentInParent<Player>();
            swordStateMachine.SwitchState(swordStateMachine.onPlayerState);
        }
    }
}
