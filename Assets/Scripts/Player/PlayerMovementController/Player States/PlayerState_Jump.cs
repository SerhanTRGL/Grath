using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Jump : PlayerState{
    private Player player;

    public override void EnterState(PlayerStateMachine playerStateMachine){
        Debug.Log("Jump");
        player = playerStateMachine.Player;
        //Animation logic, move somewhere else
        player.CharacterAnimator.SetBool("isJumping", true);
        //------------------------------------
        
        //Dust effect, move somewhere else
        player.MovementDustParticleSystem.Play();
        //--------------------------------

        
        player.RigidBody.velocity = new Vector2(player.RigidBody.velocity.x, player.JumpSpeed);
        playerStateMachine.hasJumped = true;

    }

    public override void ExitState(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        if(player.RigidBody.velocity.y < 0.1f){
            playerStateMachine.Player.CharacterAnimator.SetBool("isJumping", false);
        }
        //------------------------------------
    }

    protected override void HandleStateLogic(PlayerStateMachine playerStateMachine){
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if(horizontalInput != 0){
            player.RigidBody.velocity = new Vector2(horizontalInput * player.Speed, player.RigidBody.velocity.y);
        }
        else{
            player.RigidBody.velocity *= new Vector2(0.95f, 1);
        }
        
        //Look right
        if(horizontalInput > 0){
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        //Look left
        if(horizontalInput < 0){
            player.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    protected override void HandleStateSwitchLogic(PlayerStateMachine playerStateMachine){
        if(player.RigidBody.velocity.y < 0.01f){
            playerStateMachine.SwitchState(playerStateMachine.inAirState);
        }
        
        bool dashKeyPressed = Input.GetKeyDown(KeyCode.LeftShift);
        if(dashKeyPressed){
            playerStateMachine.SwitchState(playerStateMachine.dashState);
        }
        if(player.Health == 0){
            playerStateMachine.SwitchState(playerStateMachine.deadState);
        }
    }
}
