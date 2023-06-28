using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable{
    [field: SerializeField] public int Health { get; set; }
    [field: SerializeField] public float PlayerSpeed{get; private set;}
    [field: SerializeField] public float PlayerDashDuration{get; private set;}
    [field: SerializeField] public float PlayerDashQuotient{get; private set;}
    [field: SerializeField] public float PlayerJumpSpeed{get; private set;}
    [field: SerializeField] public bool HasJumped{get; set;}
    
    public string PlayerName;
    public Animator CharacterAnimator;
    public Transform SwordMountPoint;
    public Rigidbody2D PlayerRigidBody{get; private set;}
    public ParticleSystem MovementDustParticleSystem{get; private set;}
    public Sword PlayerSword = null;
    private PlayerStateMachine m_playerStateMachine = new PlayerStateMachine();
    void Awake() {
        this.PlayerRigidBody = GetComponent<Rigidbody2D>();
        this.MovementDustParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    void Start(){
        this.PlayerSpeed = 15f;
        this.Health = 100;
        this.PlayerDashDuration = 0.2f;
        this.PlayerDashQuotient = 3;
        this.PlayerJumpSpeed = 20f;
        this.HasJumped = false;
        m_playerStateMachine.StartStateMachine(this);
    }

    // Update is called once per frame
    void Update(){
        m_playerStateMachine.ExecuteCurrentState();
    }

    #region IDamageable Related    
    public void TakeDamage(int damage){
        this.Health = damage > this.Health ? 0 : this.Health-damage;
    }
    #endregion

    private void OnDrawGizmos() {
        Gizmos.DrawRay(this.transform.position, -this.transform.up);
        Vector3 offset = new Vector3(0, 0.85f, 0);
        Gizmos.DrawCube(PlayerRigidBody.transform.position - offset, new Vector3(1,0.1f,1));
        Debug.DrawRay(transform.position, -transform.up, Color.green);
    }
}
