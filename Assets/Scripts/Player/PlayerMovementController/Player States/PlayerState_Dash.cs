using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Dash : PlayerState{
    private Player player;
    private float _dashTimer;
    
    public PlayerState_Dash(PlayerStatusWatcher playerStatusWatcher) : base(playerStatusWatcher){
        return;
    }

    public override void EnterState(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        playerStateMachine.Player.CharacterAnimator.SetBool("isDashing", true);
        //------------------------------------

        player = playerStateMachine.Player;
        _dashTimer = 0f;
    }

    public override void ExitState(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        playerStateMachine.Player.CharacterAnimator.SetBool("isDashing", false);
        //------------------------------------
    }

    protected override void HandleStateLogic(PlayerStateMachine playerStateMachine){
        //Animation logic, move somewhere else
        if(player.RigidBody.velocity.y < 0.1f){
            playerStateMachine.Player.CharacterAnimator.SetBool("isJumping", false);
        }
        //------------------------------------
        
        float lookDirection = player.transform.rotation.eulerAngles.y == 0 ? 1 : -1;
        float velocityX = _dashTimer <= player.DashDuration ? player.Speed * lookDirection * player.DashQuotient : 0;
        float velocityY = player.RigidBody.velocity.y;

        player.RigidBody.velocity = new Vector2(velocityX, velocityY);
        _dashTimer += Time.deltaTime;
    }

    protected override void HandleStateSwitchLogic(PlayerStateMachine playerStateMachine){
        _playerStatusWatcher.isDashDone = _dashTimer >= player.DashDuration;

        if(_playerStatusWatcher.isDashDone){
            if (_playerStatusWatcher.isInAir){
                playerStateMachine.SwitchState(playerStateMachine.inAirState);
            }
            if (_playerStatusWatcher.isStandingOnGround){
                playerStateMachine.SwitchState(playerStateMachine.idleState);
            }
            if (_playerStatusWatcher.isRunning){
                playerStateMachine.SwitchState(playerStateMachine.runningState);
            }
        }

        if (_playerStatusWatcher.isJumping){//Player jumps during dash and not in air
            playerStateMachine.SwitchState(playerStateMachine.jumpState);
        }
    }
}
