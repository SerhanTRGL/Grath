using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Running : PlayerState{
    private Rigidbody2D m_playerRigidBody;
    private float m_playerSpeed;
    private float m_horizontalInput;
    public override void EnterState(PlayerStateMachine playerStateMachine){
        playerStateMachine.m_player.CharacterAnimator.SetBool("isRunning", true);
        m_playerRigidBody = playerStateMachine.m_player.PlayerRigidBody;
        m_playerSpeed = playerStateMachine.m_player.PlayerSpeed;
        playerStateMachine.m_player.HasJumped = false;
        Debug.Log("Entered player state: running");
    }

    public override void ExecuteState(PlayerStateMachine playerStateMachine){
        Debug.Log("Running");
        HandleStateLogic(playerStateMachine);
        HandleStateSwitchLogic(playerStateMachine);
    }

    public override void ExitState(PlayerStateMachine playerStateMachine){
        playerStateMachine.m_player.CharacterAnimator.SetBool("isRunning", false);
        Debug.Log("Exiting player state: running");
    }

    protected override void HandleStateLogic(PlayerStateMachine playerStateMachine){
        if(m_playerRigidBody.velocity.y < 0.1f){
            playerStateMachine.m_player.CharacterAnimator.SetBool("isJumping", false);
        }
        playerStateMachine.m_player.MovementDustParticleSystem.Play();

        m_horizontalInput = Input.GetAxisRaw("Horizontal");
        
        if(m_horizontalInput != 0){
            m_playerRigidBody.velocity = new Vector2(m_horizontalInput * m_playerSpeed, m_playerRigidBody.velocity.y);
        }
        else{ //slow down
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
        //Player stopped moving
        if(m_playerRigidBody.velocity == Vector2.zero){
            playerStateMachine.SwitchState(playerStateMachine.idleState);
        }

        //Player pressed dash key
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            playerStateMachine.SwitchState(playerStateMachine.dashState);
        }
        //Player pressed jump keys
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)){
            playerStateMachine.SwitchState(playerStateMachine.jumpState);
        }

        //Player falls of a cliff
        int groundLayerMask = LayerMask.GetMask("Ground");
        Vector2 direction = -m_playerRigidBody.transform.up;
        Debug.DrawRay(m_playerRigidBody.transform.position, -m_playerRigidBody.transform.up, Color.green);
        Vector3 offset = new Vector3(0, 0.85f, 0);
        RaycastHit2D hit = Physics2D.BoxCast(
                                            m_playerRigidBody.transform.position-offset,
                                            new Vector2(1, 0.1f), 
                                            0,
                                            -m_playerRigidBody.transform.up,
                                            1,
                                            groundLayerMask);
        bool isInAir = (hit.collider == null);
        
        if(isInAir){
            playerStateMachine.SwitchState(playerStateMachine.inAirState);
        }
    }
}
