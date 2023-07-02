using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BossPart_Head))]
[RequireComponent(typeof(BossPart_Torso))]
[RequireComponent(typeof(BossPart_Arms))]
[RequireComponent(typeof(BossPart_Legs))]
public class Boss : MonoBehaviour{
    
    public BossPart_Head BossHead;
    public BossPart_Torso BossTorso;
    public BossPart_Arms BossArms;
    public BossPart_Legs BossLegs;
    
    private int MaxHealth;
    public float HealthNormalized{
        get{
            return Health/MaxHealth;
        }
    }
    public int Health{
        get{
            return BossHead.Health + BossTorso.Health + BossArms.Health + BossLegs.Health;
        }
    }
    
    void Start(){
        BossHead = GetComponent<BossPart_Head>();
        BossTorso = GetComponent<BossPart_Torso>();
        BossArms = GetComponent<BossPart_Arms>();
        BossLegs = GetComponent<BossPart_Legs>();
        MaxHealth = Health;
        Debug.Log(Health.ToString());
        Debug.Log(HealthNormalized.ToString());
    }

    // Update is called once per frame
    void Update(){
        
    }
}
