using UnityEngine;

public class PlayerState_Dead : PlayerState{
    public override void EnterState(PlayerStateMachine playerStateMachine){
        playerStateMachine.Player.CharacterAnimator.SetBool("isDead", true);
    }

    public override void ExitState(PlayerStateMachine playerStateMachine){
        playerStateMachine.Player.CharacterAnimator.SetBool("isDead", false);
    }

    float stayDeadDuration = 1f;
    float timer = 0;
    protected override void HandleStateLogic(PlayerStateMachine playerStateMachine){
        timer += Time.deltaTime;
    }

    protected override void HandleStateSwitchLogic(PlayerStateMachine playerStateMachine){
        if(timer >= stayDeadDuration){
            playerStateMachine.SwitchState(playerStateMachine.idleState);
        }
    }
}
