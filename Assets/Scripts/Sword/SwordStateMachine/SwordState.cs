public abstract class SwordState{
    public abstract void EnterState(SwordStateMachine swordStateMachine);
    public void ExecuteState(SwordStateMachine swordStateMachine){
        HandleStateLogic(swordStateMachine);
        HandleStateSwitchLogic(swordStateMachine);
    }
    public abstract void ExitState(SwordStateMachine swordStateMachine);
    protected abstract void HandleStateLogic(SwordStateMachine swordStateMachine);
    protected abstract void HandleStateSwitchLogic(SwordStateMachine swordStateMachine);
}
