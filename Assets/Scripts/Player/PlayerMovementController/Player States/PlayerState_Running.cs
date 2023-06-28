using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Running : PlayerState{
    private Rigidbody2D m_playerRigidBody;
    private float m_playerSpeed;
    private float m_horizontalInput;
    public override void EnterState(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        playerStateMachine.Player.CharacterAnimator.SetBool("isRunning", true);
        //------------------------------------

        Player player = playerStateMachine.Player;
        
        m_playerRigidBody = player.PlayerRigidBody;
        m_playerSpeed = player.PlayerSpeed;
        player.HasJumped = false;
    }

    public override void ExitState(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        playerStateMachine.Player.CharacterAnimator.SetBool("isRunning", false);
        //------------------------------------
    }

    protected override void HandleStateLogic(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        if(m_playerRigidBody.velocity.y < 0.1f){
            playerStateMachine.Player.CharacterAnimator.SetBool("isJumping", false);
        }
        //------------------------------------

        //Dust effect, move somewhere else
        playerStateMachine.Player.MovementDustParticleSystem.Play();
        //--------------------------------

        m_horizontalInput = Input.GetAxisRaw("Horizontal");
        bool isMoving = m_horizontalInput != 0;
        
        if(isMoving){
            m_playerRigidBody.velocity = new Vector2(m_horizontalInput * m_playerSpeed, m_playerRigidBody.velocity.y);
        }
        else{ //Stopped moving, slow down
            m_playerRigidBody.velocity *= new Vector2(0.95f, 1);
        }
        
        //Look right
        if(m_horizontalInput > 0){
            m_playerRigidBody.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        //Look left
        if(m_horizontalInput < 0){
            m_playerRigidBody.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    protected override void HandleStateSwitchLogic(PlayerStateMachine playerStateMachine){
        bool isNotMoving = m_playerRigidBody.velocity == Vector2.zero;
        bool dashKeyPressed = Input.GetKeyDown(KeyCode.LeftShift);
        bool jumpKeyPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W);
        bool isInAir = IsInAir();

        if (isNotMoving){
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
