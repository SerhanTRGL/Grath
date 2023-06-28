using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState{
    public abstract void EnterState(PlayerStateMachine playerStateMachine);
    public abstract void ExecuteState(PlayerStateMachine playerStateMachine);
    public abstract void ExitState(PlayerStateMachine playerStateMachine);
    protected abstract void HandleStateLogic(PlayerStateMachine playerStateMachine);
    protected abstract void HandleStateSwitchLogic(PlayerStateMachine playerStateMachine);
}
