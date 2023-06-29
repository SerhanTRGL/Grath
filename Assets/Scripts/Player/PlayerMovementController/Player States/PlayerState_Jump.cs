using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Jump : PlayerState{
    private Player player;

    public PlayerState_Jump(PlayerStatusWatcher playerStatusWatcher) : base(playerStatusWatcher){
        return;
    }

    public override void EnterState(PlayerStateMachine playerStateMachine){
        player = playerStateMachine.Player;
        //Animation logic, move somewhere else
        player.CharacterAnimator.SetBool("isJumping", true);
        //------------------------------------
        
        //Dust effect, move somewhere else
        player.MovementDustParticleSystem.Play();
        //--------------------------------

        
        player.RigidBody.velocity = new Vector2(player.RigidBody.velocity.x, player.JumpSpeed);
        _playerStatusWatcher.hasJumped = true;

    }

    public override void ExitState(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        if(player.RigidBody.velocity.y < 0.1f){
            playerStateMachine.Player.CharacterAnimator.SetBool("isJumping", false);
        }
        //------------------------------------
    }

    protected override void HandleStateLogic(PlayerStateMachine playerStateMachine){
    }

    protected override void HandleStateSwitchLogic(PlayerStateMachine playerStateMachine){
        playerStateMachine.SwitchState(playerStateMachine.inAirState);
    }
}
