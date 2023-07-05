using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour{
    
    public BossPart_Head BossHead;
    public BossPart_Torso BossTorso;
    public BossPart_Arms BossArms;
    
    private int MaxHealth;
    public float HealthNormalized{
        get{
            return Health/MaxHealth;
        }
    }
    public int Health{
        get{
            return BossHead.Health + BossTorso.Health + BossArms.Health;
        }
    }
    
    void Start(){ 
        MaxHealth = Health;
        Debug.Log(Health.ToString());
        Debug.Log(HealthNormalized.ToString());
    }

    // Update is called once per frame
    void Update(){
        
    }
}
