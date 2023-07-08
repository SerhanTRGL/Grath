using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Fall stunned state add
public partial class Player : MonoBehaviour{
    public static event Action OnPlayerDied;
    public static event Action OnPlayerHealthChanged;
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
        this.Health = this.MaxHealth;
        this.DashDuration = 0.2f;
        this.DashQuotient = 3;
        this.JumpSpeed = 20f;

        this.GetComponentInChildren<CharacterGenerator>().GenerateNewCharacter();

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
    [field: SerializeField] public int MaxHealth{get; set;}
    [field: SerializeField] public float InvincibilityDuration { get; set; }
    
    public IEnumerator MakeInvincible(){
        GetComponentInChildren<CapsuleCollider2D>().enabled = false;
        yield return new WaitForSeconds(InvincibilityDuration);
        GetComponentInChildren<CapsuleCollider2D>().enabled = true;
    }

    public void TakeDamage(int damage){
        this.Health = damage > this.Health ? 0 : this.Health-damage;
        StartCoroutine(MakeInvincible());
        OnPlayerHealthChanged?.Invoke();
        if(Health == 0){
            OnPlayerDied?.Invoke();
        }
    }
}

public partial class Player : ISwordHolder{
    public Transform SwordHoldPoint;
    public Sword Sword{
        get;
        set;
    }

    public void AcquireSword(Sword swordToAcquire){
        if(Sword == null){
            swordToAcquire.transform.parent = SwordHoldPoint.transform;
            swordToAcquire.transform.localPosition = Vector3.zero;
            swordToAcquire.transform.localRotation = Quaternion.Euler(0,0,0);
        }
        if(Sword != null){
            Destroy(Sword.gameObject);
            swordToAcquire.transform.parent = SwordHoldPoint.transform;
            swordToAcquire.transform.localPosition = Vector3.zero;
            swordToAcquire.transform.localRotation = Quaternion.Euler(0,0,0);
        }
        Sword = swordToAcquire;
    }

    public void DropSword()
    {
        throw new System.NotImplementedException();
    }
}