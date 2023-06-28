using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Jump : PlayerState{
    private Rigidbody2D m_playerRigidBody;
    private float m_playerJumpSpeed;
    public override void EnterState(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        playerStateMachine.Player.CharacterAnimator.SetBool("isJumping", true);
        //------------------------------------

        Player player = playerStateMachine.Player;
        m_playerRigidBody = player.PlayerRigidBody;
        m_playerJumpSpeed = player.PlayerJumpSpeed;
        

        //Dust effect, move somewhere else
        playerStateMachine.Player.MovementDustParticleSystem.Play();
        //--------------------------------
    }

    public override void ExitState(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        if(m_playerRigidBody.velocity.y < 0.1f){
            playerStateMachine.Player.CharacterAnimator.SetBool("isJumping", false);
        }
        //------------------------------------
    }

    protected override void HandleStateLogic(PlayerStateMachine playerStateMachine){
        bool hasNotJumped = !playerStateMachine.Player.HasJumped;
        
        if(hasNotJumped){
            m_playerRigidBody.velocity = new Vector2(m_playerRigidBody.velocity.x, m_playerJumpSpeed);
            playerStateMachine.Player.HasJumped = true;
        }
    }

    protected override void HandleStateSwitchLogic(PlayerStateMachine playerStateMachine){
        playerStateMachine.SwitchState(playerStateMachine.inAirState);
    }
}
