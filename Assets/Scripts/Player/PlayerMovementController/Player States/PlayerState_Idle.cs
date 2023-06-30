using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Idle : PlayerState{
    private Player player;

    public override void EnterState(PlayerStateMachine playerStateMachine){
        Debug.Log("Idle");
        //Animation logic, move somewhere else
        playerStateMachine.Player.CharacterAnimator.SetBool("isIdle", true);
        //-----------------------------------

        player = playerStateMachine.Player;
        playerStateMachine.hasJumped = false;
    }

    public override void ExitState(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        player.CharacterAnimator.SetBool("isIdle", false);
        //-----------------------------------
    }

    protected override void HandleStateLogic(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        if(player.RigidBody.velocity.y < 0.1f){
            player.CharacterAnimator.SetBool("isJumping", false);
        }
        //-----------------------------------
    }

    protected override void HandleStateSwitchLogic(PlayerStateMachine playerStateMachine){
        bool dashKeyPressed = Input.GetKeyDown(KeyCode.LeftShift);
        bool jumpKeyPressed = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space);
        bool horizontalInputReceived = Input.GetAxisRaw("Horizontal") != 0;
        
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, -player.transform.up, 1, LayerMask.GetMask("Ground"));
        bool isInAir = (hit.collider == null); 
        
        if(dashKeyPressed){ 
            playerStateMachine.SwitchState(playerStateMachine.dashState);
        }
        if(jumpKeyPressed){    
            playerStateMachine.SwitchState(playerStateMachine.jumpState);
        }
        if(horizontalInputReceived){
            playerStateMachine.SwitchState(playerStateMachine.runningState);
        }
        if(isInAir){
            playerStateMachine.SwitchState(playerStateMachine.inAirState);
        }
    }
}
