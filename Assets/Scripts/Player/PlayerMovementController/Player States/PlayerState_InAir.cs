using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_InAir : PlayerState{
    private float m_playerSpeed;
    private float m_horizontalInput;
    private Rigidbody2D m_playerRigidBody;
    public override void EnterState(PlayerStateMachine playerStateMachine){
        playerStateMachine.Player.CharacterAnimator.SetBool("isInAir", true);
        m_playerRigidBody = playerStateMachine.Player.PlayerRigidBody;
        m_playerSpeed = playerStateMachine.Player.PlayerSpeed;
        Debug.Log("Entered player state: inAir");
    }

    public override void ExitState(PlayerStateMachine playerStateMachine){
        playerStateMachine.Player.CharacterAnimator.SetBool("isInAir", false);
        Debug.Log("Exiting player state: inAir");
    }

    protected override void HandleStateLogic(PlayerStateMachine playerStateMachine){
        if(m_playerRigidBody.velocity.y < 0.1f){
            playerStateMachine.Player.CharacterAnimator.SetBool("isJumping", false);
        }
        m_horizontalInput = Input.GetAxisRaw("Horizontal");
        
        
        if(m_horizontalInput != 0){
            m_playerRigidBody.velocity = new Vector2(m_horizontalInput * m_playerSpeed, m_playerRigidBody.velocity.y);
        }
        else{
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
        
        if(!isInAir && m_playerRigidBody.velocity.y == 0){//Player touches the ground again
            Debug.Log(hit.collider.name);
            playerStateMachine.SwitchState(playerStateMachine.idleState);
        }

        //Player pressed dash key
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            playerStateMachine.SwitchState(playerStateMachine.dashState);
        }

       //Player pressed jump keys and has not jumped before going into this state
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && !playerStateMachine.Player.HasJumped){
            playerStateMachine.SwitchState(playerStateMachine.jumpState);
        }
    }
}
