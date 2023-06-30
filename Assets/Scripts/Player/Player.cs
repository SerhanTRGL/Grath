using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable{
    #region IDamageable Related
    [field: SerializeField] public int Health { get; set; }    
    public void TakeDamage(int damage){
        this.Health = damage > this.Health ? 0 : this.Health-damage;
    }
    #endregion
    
    public string CharacterName;
    public float Speed;
    public float DashDuration;
    public float DashQuotient;
    public float JumpSpeed;
    
    #region References
    public Animator CharacterAnimator;
    public Transform SwordMountPoint;
    public Rigidbody2D RigidBody;
    public ParticleSystem MovementDustParticleSystem;
    public Sword PlayerSword = null;
    #endregion

    private PlayerStateMachine m_playerStateMachine = new PlayerStateMachine();
    void Awake() {
        this.RigidBody = GetComponent<Rigidbody2D>();
        this.MovementDustParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    void Start(){
        this.Speed = 15f;
        this.Health = 100;
        this.DashDuration = 0.2f;
        this.DashQuotient = 3;
        this.JumpSpeed = 20f;
        
        m_playerStateMachine.StartStateMachine(this);
    }

    void Update(){
        m_playerStateMachine.ExecuteCurrentState();
    }

    private void OnDrawGizmos() {
        Gizmos.DrawRay(this.transform.position, -this.transform.up);
    }
}
