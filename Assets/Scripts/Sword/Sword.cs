using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour{
    public bool debugOn = true;
    private SwordStateMachine m_swordStateMachine = new SwordStateMachine();
    public ComboManager ComboManager{get; private set;}
    public Animator PlayerAnimator;
    public bool IsOwnedByPlayer{
        get => transform.parent != null && transform.parent.tag == "Player";
    }
    public bool IsOwnedByBoss{
        get => transform.parent != null && transform.parent.tag == "Boss";
    }
    public bool IsOnGround{
        get => transform.parent == null;
    }
    private void Awake() {
        this.ComboManager = GetComponent<ComboManager>();
    }
    void Start(){
        m_swordStateMachine.StartStateMachine(this);
    }

    void Update(){
        m_swordStateMachine.ExecuteCurrentState();
    }
    private void OnDrawGizmos() {
        if(debugOn){
            Gizmos.DrawCube(transform.position, new Vector2(5, 5));
        }
    }
}
