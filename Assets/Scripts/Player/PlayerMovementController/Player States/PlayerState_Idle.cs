using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Idle : PlayerState{
    private Player player;
    public PlayerState_Idle(PlayerStatusWatcher playerStatusWatcher) : base(playerStatusWatcher){
        return;
    }

    public override void EnterState(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        playerStateMachine.Player.CharacterAnimator.SetBool("isIdle", true);
        //-----------------------------------

        player = playerStateMachine.Player;
        _playerStatusWatcher.hasJumped = false;
    }

    public override void ExitState(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        player.CharacterAnimator.SetBool("isIdle", false);
        //-----------------------------------
    }

    protected override void HandleStateLogic(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        if(player.RigidBody.velocity.y < 0.1f){
            player.CharacterAnimator.SetBool("isJumping", false);
        }
        //-----------------------------------
    }

    protected override void HandleStateSwitchLogic(PlayerStateMachine playerStateMachine){
        if(_playerStatusWatcher.dashKeyPressed){ 
            playerStateMachine.SwitchState(playerStateMachine.dashState);
        }
        if(_playerStatusWatcher.jumpKeyPressed){    
            playerStateMachine.SwitchState(playerStateMachine.jumpState);
        }
        if(_playerStatusWatcher.isMoving){
            playerStateMachine.SwitchState(playerStateMachine.runningState);
        }
    }
}
