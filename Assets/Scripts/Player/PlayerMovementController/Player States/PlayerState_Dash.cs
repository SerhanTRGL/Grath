using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Dash : PlayerState{
    private Rigidbody2D m_playerRigidBody;
    private float m_dashDuration;
    private float m_dashTimer;
    private float m_dashQuotient;
    private float m_playerLookDirection;
    private float m_playerSpeed;
    
    public override void EnterState(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        playerStateMachine.Player.CharacterAnimator.SetBool("isDashing", true);
        //------------------------------------

        Player player = playerStateMachine.Player;
        m_playerRigidBody = player.PlayerRigidBody;
        m_dashDuration = player.PlayerDashDuration;
        m_dashTimer = 0f;
        m_playerSpeed = player.PlayerSpeed;
        m_playerLookDirection = player.PlayerRigidBody.transform.rotation.eulerAngles.y == 0 ? 1 : -1;
        m_dashQuotient = player.PlayerDashQuotient;
    }

    public override void ExitState(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        playerStateMachine.Player.CharacterAnimator.SetBool("isDashing", false);
        //------------------------------------
    }

    protected override void HandleStateLogic(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        if(m_playerRigidBody.velocity.y < 0.1f){
            playerStateMachine.Player.CharacterAnimator.SetBool("isJumping", false);
        }
        //------------------------------------
        
        float velocityY = m_playerRigidBody.velocity.y;
        float velocityX = m_dashTimer <= m_dashDuration ? m_playerSpeed * m_playerLookDirection * m_dashQuotient : 0;

        m_playerRigidBody.velocity = new Vector2(velocityX, velocityY);
        m_dashTimer += Time.deltaTime;
    }

    protected override void HandleStateSwitchLogic(PlayerStateMachine playerStateMachine){
        int groundLayerMask = LayerMask.GetMask("Ground");
        Vector2 direction = -m_playerRigidBody.transform.up;
        Debug.DrawRay(m_playerRigidBody.transform.position, -m_playerRigidBody.transform.up, Color.green);
        
        Vector3 offset = new Vector3(0, 0.85f, 0);
        RaycastHit2D hit = Physics2D.BoxCast(
                                            m_playerRigidBody.transform.position-offset,
                                            new Vector2(1,0.1f),
                                            0,
                                            -m_playerRigidBody.transform.up,
                                            1,
                                            groundLayerMask);
        bool isInAir = (hit.collider == null);

        if(m_dashTimer >= m_dashDuration){
            if(isInAir){//Player still in air after dash is complete
            playerStateMachine.SwitchState(playerStateMachine.inAirState);
            }
            if(m_playerRigidBody.velocity == Vector2.zero && !isInAir){ //player stopped moving and not in air
                playerStateMachine.SwitchState(playerStateMachine.idleState);
            }
            if(Input.GetAxisRaw("Horizontal") != 0 || m_playerRigidBody.velocity.x != 0 && m_playerRigidBody.velocity.y == 0){ //Player still running after dash
                playerStateMachine.SwitchState(playerStateMachine.runningState);
            }
        }
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) && !isInAir && !playerStateMachine.Player.HasJumped){//Player jumps during dash and not in air
            playerStateMachine.SwitchState(playerStateMachine.jumpState);
        }
    }
}
