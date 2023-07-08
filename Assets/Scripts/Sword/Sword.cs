using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour{
    private SwordStateMachine m_swordStateMachine = new SwordStateMachine();
    public ComboManager ComboManager{get; private set;}
    public Animator PlayerAnimator;
    public Player Holder;
    public int damage;
    private void Awake() {
        this.ComboManager = GetComponent<ComboManager>();
    }
    void Start(){
        m_swordStateMachine.StartStateMachine(this);
    }

    void Update(){
        m_swordStateMachine.ExecuteCurrentState();
    }
}
