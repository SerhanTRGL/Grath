using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Dash : PlayerState{
    private Player player;
    private float _dashTimer;

    public override void EnterState(PlayerStateMachine playerStateMachine){
        Debug.Log("Dash");
        //Animation logic, move somewhere else
        playerStateMachine.Player.CharacterAnimator.SetBool("isDashing", true);
        //------------------------------------

        player = playerStateMachine.Player;
        _dashTimer = 0f;
    }

    public override void ExitState(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        playerStateMachine.Player.CharacterAnimator.SetBool("isDashing", false);
        //------------------------------------
    }

    protected override void HandleStateLogic(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        if(player.RigidBody.velocity.y < 0.1f){
            playerStateMachine.Player.CharacterAnimator.SetBool("isJumping", false);
        }
        //------------------------------------
        
        float lookDirection = player.transform.rotation.eulerAngles.y == 0 ? 1 : -1;
        float velocityX = _dashTimer <= player.DashDuration ? player.Speed * lookDirection * player.DashQuotient : 0;
        float velocityY = player.RigidBody.velocity.y;

        player.RigidBody.velocity = new Vector2(velocityX, velocityY);
        _dashTimer += Time.deltaTime;
    }

    protected override void HandleStateSwitchLogic(PlayerStateMachine playerStateMachine){
        bool isDashDone = _dashTimer >= player.DashDuration;
        bool isStationary = player.RigidBody.velocity == Vector2.zero;
        bool jumpKeyPressed = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space);
        bool horizontalInputReceived = Input.GetAxisRaw("Horizontal") != 0;
        
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, -player.transform.up, 1, LayerMask.GetMask("Ground"));
        bool isInAir = (hit.collider == null); 

        if(isDashDone){
            if (isInAir){
                playerStateMachine.SwitchState(playerStateMachine.inAirState);
            }
            if (isStationary){
                playerStateMachine.SwitchState(playerStateMachine.idleState);
            }
            if (horizontalInputReceived){
                playerStateMachine.SwitchState(playerStateMachine.runningState);
            }
        }

        if (jumpKeyPressed && !isInAir){//Player jumps during dash and not in air
            playerStateMachine.SwitchState(playerStateMachine.jumpState);
        }
    }
}
