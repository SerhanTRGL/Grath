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
        bool isDashDone = m_dashTimer >= m_dashDuration;
        bool isInAir = this.IsInAir();
        bool isStandingOnGround = m_playerRigidBody.velocity == Vector2.zero && !isInAir;
        bool isRunning = Input.GetAxisRaw("Horizontal") != 0 || m_playerRigidBody.velocity.x != 0 && m_playerRigidBody.velocity.y == 0;
        bool isJumping = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) && !isInAir && !playerStateMachine.Player.HasJumped;

        if (isDashDone){
            if (isInAir){
                playerStateMachine.SwitchState(playerStateMachine.inAirState);
            }
            if (isStandingOnGround){
                playerStateMachine.SwitchState(playerStateMachine.idleState);
            }
            if (isRunning){
                playerStateMachine.SwitchState(playerStateMachine.runningState);
            }
        }
        
        if (isJumping){//Player jumps during dash and not in air
            playerStateMachine.SwitchState(playerStateMachine.jumpState);
        }
    }

    private bool IsInAir(){
        int groundLayerMask = LayerMask.GetMask("Ground");

        Vector2 direction = -m_playerRigidBody.transform.up;
        Vector3 offset = new Vector3(0, 0.85f, 0);
        Vector3 origin = m_playerRigidBody.transform.position - offset;
        Vector2 size = new Vector2(1, 0.1f);
        float angle = 0f;
        float distance = 1f;
        RaycastHit2D hit = Physics2D.BoxCast(origin, size, angle, direction, distance, groundLayerMask);
        bool isInAir = (hit.collider == null);
        return isInAir;
    }
}
