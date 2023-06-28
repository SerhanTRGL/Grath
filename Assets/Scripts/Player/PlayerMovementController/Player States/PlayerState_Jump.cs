using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Jump : PlayerState{
    private Rigidbody2D m_playerRigidBody;
    private float m_playerJumpSpeed;
    public override void EnterState(PlayerStateMachine playerStateMachine){
        playerStateMachine.Player.CharacterAnimator.SetBool("isJumping", true);

        m_playerRigidBody = playerStateMachine.Player.PlayerRigidBody;
        m_playerJumpSpeed = playerStateMachine.Player.PlayerJumpSpeed;
        m_playerRigidBody.velocity = new Vector2(m_playerRigidBody.velocity.x, m_playerJumpSpeed);
        playerStateMachine.Player.HasJumped = true;
        playerStateMachine.Player.MovementDustParticleSystem.Play();
        Debug.Log("Entered player state: jump");
    }

    public override void ExitState(PlayerStateMachine playerStateMachine){
        if(m_playerRigidBody.velocity.y < 0.1f){
            playerStateMachine.Player.CharacterAnimator.SetBool("isJumping", false);
        }
        Debug.Log("Exit player state: jump");
    }

    protected override void HandleStateLogic(PlayerStateMachine playerStateMachine){
        
    }

    protected override void HandleStateSwitchLogic(PlayerStateMachine playerStateMachine){
        playerStateMachine.SwitchState(playerStateMachine.inAirState);
    }
}
