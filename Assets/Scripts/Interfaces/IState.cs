public interface IState{
    void EnterState();      //What to do at first
    void ExecuteState();    //What to do continuously
    void ExitState();       //What to do on exit
    void HandleStateLogic(); //What is the logic of the state
    void HandleStateSwitchLogic(); //How will the state be changed
}   
