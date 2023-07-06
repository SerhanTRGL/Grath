using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BossPart_Torso : MonoBehaviour{
    // Start is called before the first frame update
    private void Awake() {
        Health = MaxHealth;
    }
    void Start(){
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.Alpha0)){
            StartCoroutine(Attack_State1());
        }
    }
}

public partial class BossPart_Torso : IDamageable{
    [Header("Part Attributes")]
    public int MaxHealth;
    public float HealthNormalized{
        get{
            return Health/MaxHealth;
        }
    }
    public int Health { 
        get;  
        set;
    }

    public void TakeDamage(int damage){
        this.Health = damage >= Health ? 0 : Health-damage;
    }
}

public partial class BossPart_Torso : IBossAttack{  
    public Transform AttackTarget { get => attackTarget; set => attackTarget = value; }
    
    [Header("General")]
    public Transform attackTarget;
    public GameObject fireballPrefab;
    public Transform firePoint;

    [Header("State 1")]
    public int numberOfFireballsInCircle;
    public float fireballMoveSpeed;
    public float stateCooldownTime;
    public IEnumerator Attack_State1(){
        for(int i = 0; i < numberOfFireballsInCircle; i++){
            float radians = (2 * Mathf.PI / numberOfFireballsInCircle) * i;

            float verticalDir = Mathf.Sin(radians);
            float horizontalDir = Mathf.Cos(radians);

            Vector3 moveDir = new Vector3(horizontalDir, verticalDir);
            Fireball fireball = Instantiate(fireballPrefab, firePoint).GetComponent<Fireball>();
            fireball.sticksToGround = false;

            fireball.rb.velocity = moveDir * fireballMoveSpeed;
        }
        yield return new WaitForSeconds(stateCooldownTime);
    }

    public IEnumerator Attack_State2()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator Attack_State3()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator Attack_State4()
    {
        throw new System.NotImplementedException();
    }
}
