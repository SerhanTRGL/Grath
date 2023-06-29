using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Running : PlayerState{
    private Player player;
    public PlayerState_Running(PlayerStatusWatcher playerStatusWatcher) : base(playerStatusWatcher){
        return;
    }

    public override void EnterState(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        playerStateMachine.Player.CharacterAnimator.SetBool("isRunning", true);
        //------------------------------------

        player = playerStateMachine.Player;
        _playerStatusWatcher.hasJumped = false;
    }

    public override void ExitState(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        player.CharacterAnimator.SetBool("isRunning", false);
        //------------------------------------
    }

    protected override void HandleStateLogic(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        if(player.RigidBody.velocity.y < 0.1f){
            player.CharacterAnimator.SetBool("isJumping", false);
        }
        //------------------------------------

        //Dust effect, move somewhere else
        player.MovementDustParticleSystem.Play();
        //--------------------------------

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if(_playerStatusWatcher.isMoving){
            player.RigidBody.velocity = new Vector2(horizontalInput * player.Speed, player.RigidBody.velocity.y);
        }
        else{
            player.RigidBody.velocity *= new Vector2(0.95f, 1);
        }
        
        //Look right
        if(horizontalInput > 0){
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        //Look left
        if(horizontalInput < 0){
            player.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    protected override void HandleStateSwitchLogic(PlayerStateMachine playerStateMachine){
        if (_playerStatusWatcher.isNotMoving){
            playerStateMachine.SwitchState(playerStateMachine.idleState);
        }
        if (_playerStatusWatcher.dashKeyPressed){
            playerStateMachine.SwitchState(playerStateMachine.dashState);
        }
        if (_playerStatusWatcher.jumpKeyPressed){
            playerStateMachine.SwitchState(playerStateMachine.jumpState);
        }
        if (_playerStatusWatcher.isInAir){
            playerStateMachine.SwitchState(playerStateMachine.inAirState);
        }
    }
}
