using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Fall stunned state add
//FIXME: Animations 
public partial class Player : MonoBehaviour{
    public string CharacterName;
    public float Speed;
    public float DashDuration;
    public float DashQuotient;
    public float JumpSpeed;
    
    #region References
    public Animator CharacterAnimator;
    public Rigidbody2D RigidBody;
    public ParticleSystem MovementDustParticleSystem;
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


public partial class Player: IDamageable{
    [field: SerializeField] public int Health { get; set; }    
    public void TakeDamage(int damage){
        this.Health = damage > this.Health ? 0 : this.Health-damage;
    }
}

public partial class Player : ISwordHolder{
    public Transform SwordBackPoint;
    public Transform SwordHoldPoint;
    public Sword Sword{
        get;
        set;
    }

    public void AcquireSword(Sword swordToAcquire){
        if(Sword == null){
            swordToAcquire.transform.parent = SwordBackPoint.transform;
            swordToAcquire.transform.localPosition = Vector3.zero;
            swordToAcquire.transform.localRotation = Quaternion.Euler(0,0,0);
            swordToAcquire.transform.localScale = Vector3.one;
        }
    }

    public void DropSword()
    {
        throw new System.NotImplementedException();
    }
}