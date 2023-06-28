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
        playerStateMachine.m_player.CharacterAnimator.SetBool("isDashing", true);
        Debug.Log("Entered player state: dash");
        m_playerRigidBody = playerStateMachine.m_player.PlayerRigidBody;
        m_dashDuration = playerStateMachine.m_player.PlayerDashDuration;
        m_dashTimer = 0f;
        m_playerSpeed = playerStateMachine.m_player.PlayerSpeed;
        m_playerLookDirection = playerStateMachine.m_player.PlayerRigidBody.transform.rotation.eulerAngles.y == 0 ? 1 : -1;
        m_dashQuotient = playerStateMachine.m_player.PlayerDashQuotient;
    }

    //Stay in dash state for dash duration
    public override void ExecuteState(PlayerStateMachine playerStateMachine){
        Debug.Log("Dash");
        HandleStateLogic(playerStateMachine);
        HandleStateSwitchLogic(playerStateMachine);
    }

    public override void ExitState(PlayerStateMachine playerStateMachine){
        playerStateMachine.m_player.CharacterAnimator.SetBool("isDashing", false);
        Debug.Log("Exiting player state: dash");
    }

    protected override void HandleStateLogic(PlayerStateMachine playerStateMachine){
        if(m_playerRigidBody.velocity.y < 0.1f){
            playerStateMachine.m_player.CharacterAnimator.SetBool("isJumping", false);
        }
        if(m_dashTimer <= m_dashDuration){
            m_dashTimer += Time.deltaTime;
            m_playerRigidBody.velocity = new Vector2(m_playerSpeed * m_playerLookDirection * m_dashQuotient, m_playerRigidBody.velocity.y);
        }
        else{
            m_playerRigidBody.velocity = new Vector2(0, m_playerRigidBody.velocity.y);
        }
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
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) && !isInAir && !playerStateMachine.m_player.HasJumped){//Player jumps during dash and not in air
            playerStateMachine.SwitchState(playerStateMachine.jumpState);
        }
    }
}
