using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Idle : PlayerState{
    public override void EnterState(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        playerStateMachine.Player.CharacterAnimator.SetBool("isIdle", true);
        //-----------------------------------

        playerStateMachine.Player.HasJumped = false;
    }

    public override void ExitState(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        playerStateMachine.Player.CharacterAnimator.SetBool("isIdle", false);
        //-----------------------------------
    }

    protected override void HandleStateLogic(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        if(playerStateMachine.Player.PlayerRigidBody.velocity.y < 0.1f){
            playerStateMachine.Player.CharacterAnimator.SetBool("isJumping", false);
        }
        //-----------------------------------
    }

    protected override void HandleStateSwitchLogic(PlayerStateMachine playerStateMachine){
        bool dashKeyPressed = Input.GetKeyDown(KeyCode.LeftShift);
        bool jumpKeyPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W);
        bool isMoving = Input.GetAxisRaw("Horizontal") != 0;
        
        if(dashKeyPressed){ 
            playerStateMachine.SwitchState(playerStateMachine.dashState);
        }
        if(jumpKeyPressed){    
            playerStateMachine.SwitchState(playerStateMachine.jumpState);
        }
        if(isMoving){
            playerStateMachine.SwitchState(playerStateMachine.runningState);
        }
    }
}
