using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Running : PlayerState{
    private Player player;

    public override void EnterState(PlayerStateMachine playerStateMachine){
        Debug.Log("Running");
        //Animation logic, move somewhere else
        playerStateMachine.Player.CharacterAnimator.SetBool("isRunning", true);
        //------------------------------------

        player = playerStateMachine.Player;
        playerStateMachine.hasJumped = false;
    }

    public override void ExitState(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        player.CharacterAnimator.SetBool("isRunning", false);
        //------------------------------------
    }

    protected override void HandleStateLogic(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        if(player.RigidBody.velocity.y < 0.1f){
            player.CharacterAnimator.SetBool("isJumping", false);
        }
        //------------------------------------

        //Dust effect, move somewhere else
        player.MovementDustParticleSystem.Play();
        //--------------------------------

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
        bool isStationary = player.RigidBody.velocity == Vector2.zero;
        bool dashKeyPressed = Input.GetKeyDown(KeyCode.LeftShift);
        bool jumpKeyPressed = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space);
        
        RaycastHit2D hit = Physics2D.Linecast(player.transform.position, player.transform.position - new Vector3(0, 1.1f, 0), LayerMask.GetMask("Ground"));
        Debug.DrawLine(player.transform.position, player.transform.position - new Vector3(0, 1.1f, 0), Color.blue, 0.01f);
        bool isInAir = (hit.collider == null); 

        if (isStationary){
            playerStateMachine.SwitchState(playerStateMachine.idleState);
        }
        if (dashKeyPressed){
            playerStateMachine.SwitchState(playerStateMachine.dashState);
        }
        if (jumpKeyPressed){
            playerStateMachine.SwitchState(playerStateMachine.jumpState);
        }
        if (isInAir){
            playerStateMachine.SwitchState(playerStateMachine.inAirState);
        }

        if(player.Health == 0){
            playerStateMachine.SwitchState(playerStateMachine.deadState);
        }
    }
}
