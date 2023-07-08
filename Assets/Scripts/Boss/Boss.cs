using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour{
    
    public static event Action OnBossHealthBelow75Perc;
    public static event Action OnBossHealthBelow50Perc;
    public static event Action OnBossHealthBelow25Perc;
    public static event Action OnBossHealthChanged;
    public BossPart_Head BossHead;
    public BossPart_Torso BossTorso;
    public BossPart_Arms BossArms;
    public static bool PlayerIsInBossArea = false;
    
    public static int NumberOfCurrentlyHappeningAttacks = 0;
    public static int MaximumConcurrentAttacks = 1;
    private int MaxHealth;
    public float HealthNormalized{
        get{
            return (float)Health/(float)MaxHealth;
        }
    }
    public int Health{
        get{
            return BossHead.Health + BossTorso.Health + BossArms.Health;
        }
    }
    
    void Start(){ 
        BossPart_Arms.OnBossArmsHealthChanged += BossPartTookDamage;
        BossPart_Torso.OnBossTorsoHealthChanged += BossPartTookDamage;
        BossPart_Head.OnBossHeadHealthChanged += BossPartTookDamage;

        Boss.OnBossHealthChanged += Boss_OnBossHealthChanged;
        Teleporter.OnPlayerTeleported += Teleporter_OnPlayerTeleported;
        Player.OnPlayerDied += Player_OnPlayerDied;

        MaxHealth = Health;
        OnBossHealthChanged?.Invoke();
    }

    private void BossPartTookDamage(){
        OnBossHealthChanged?.Invoke();
    }

    private void Player_OnPlayerDied(){
        PlayerIsInBossArea = false;
    }

    private void Teleporter_OnPlayerTeleported(){
        PlayerIsInBossArea = true;
    }

    private void Boss_OnBossHealthChanged(){
        Debug.Log(Health.ToString());
        if(HealthNormalized <= 0.25f){
            OnBossHealthBelow25Perc?.Invoke();
        }
        else if(HealthNormalized <= 0.50f){
            OnBossHealthBelow50Perc?.Invoke();
            MaximumConcurrentAttacks = 3;
        }
        else if(HealthNormalized <= 0.75f){
            OnBossHealthBelow75Perc?.Invoke();
            MaximumConcurrentAttacks = 2;
        }
    }

    private void OnDestroy() {
        BossPart_Arms.OnBossArmsHealthChanged -= BossPartTookDamage;
        BossPart_Torso.OnBossTorsoHealthChanged -= BossPartTookDamage;
        BossPart_Head.OnBossHeadHealthChanged -= BossPartTookDamage;
        
        Boss.OnBossHealthChanged -= Boss_OnBossHealthChanged;
        Teleporter.OnPlayerTeleported -= Teleporter_OnPlayerTeleported;
        Player.OnPlayerDied -= Player_OnPlayerDied;
    }

}
