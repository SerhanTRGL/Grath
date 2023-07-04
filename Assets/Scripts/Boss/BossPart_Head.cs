using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BossPart_Head : MonoBehaviour{
    

    // Start is called before the first frame update
    private void Awake() {
        Health = 100;
        MaxHealth = Health;
    }
    void Start(){

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public partial class BossPart_Head : IDamageable{
    private int MaxHealth;
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

public partial class BossPart_Head : IBossAttack{
    public Transform AttackTarget { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public IEnumerator Attack_State1()
    {
        throw new System.NotImplementedException();
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
