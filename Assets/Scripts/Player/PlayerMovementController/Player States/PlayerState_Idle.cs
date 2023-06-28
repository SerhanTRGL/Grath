using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Idle : PlayerState{
    public override void EnterState(PlayerStateMachine playerStateMachine){
        playerStateMachine.Player.CharacterAnimator.SetBool("isIdle", true);
        Debug.Log("Entered player state: idle");
        playerStateMachine.Player.HasJumped = false;
    }

    public override void ExitState(PlayerStateMachine playerStateMachine){
        Debug.Log("Exiting player state: idle");
        playerStateMachine.Player.CharacterAnimator.SetBool("isIdle", false);
    }

    protected override void HandleStateLogic(PlayerStateMachine playerStateMachine){
        if(playerStateMachine.Player.PlayerRigidBody.velocity.y < 0.1f){
            playerStateMachine.Player.CharacterAnimator.SetBool("isJumping", false);
        }
    }

    protected override void HandleStateSwitchLogic(PlayerStateMachine playerStateMachine){
        
        //Left Mouse Button Pressed
        //if(Input.GetMouseButton(0)){playerStateMachine.SwitchState(playerStateMachine.attack1State);}
        //Shift pressed
        if(Input.GetKeyDown(KeyCode.LeftShift)){ 
            playerStateMachine.SwitchState(playerStateMachine.dashState);
        }
        //Space pressed
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)){    
            playerStateMachine.SwitchState(playerStateMachine.jumpState);
        }
        //A or D pressed
        if(Input.GetAxisRaw("Horizontal") != 0){
            playerStateMachine.SwitchState(playerStateMachine.runningState);
        }
    }
}
