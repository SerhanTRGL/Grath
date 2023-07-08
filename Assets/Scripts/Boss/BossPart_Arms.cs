using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public partial class BossPart_Arms : MonoBehaviour{
    public static event Action OnBossArmsHealthChanged;
    private void Awake() {
        Health = MaxHealth;
        OnBossArmsHealthChanged += BossArms_OnHealthChanged;
        InvincibilityDuration = 0.1f;
    }

    private void Start() {
        //Force increase state of the part depending on boss health
        Boss.OnBossHealthBelow75Perc += Boss_OnBossHealthBelow75Perc;
        Boss.OnBossHealthBelow50Perc += Boss_OnBossHealthBelow50Perc;
        Boss.OnBossHealthBelow25Perc += Boss_OnBossHealthBelow25Perc;
    }

    public SpriteResolver upperLeftArmSpriteResolver;
    public SpriteResolver upperRightArmSpriteResolver;
    public SpriteResolver lowerLeftArmSpriteResolver;
    public SpriteResolver lowerRightArmSpriteResolver;
    public SpriteResolver leftHandSpriteResolver;
    public SpriteResolver rightHandSpriteResolver;

    private void UpdateSprites(int bossPartState){
        upperLeftArmSpriteResolver.SetCategoryAndLabel("UpperArm_Left", bossPartState.ToString());
        upperRightArmSpriteResolver.SetCategoryAndLabel("UpperArm_Right", bossPartState.ToString());
        lowerLeftArmSpriteResolver.SetCategoryAndLabel("LowerArm_Left", bossPartState.ToString());
        lowerRightArmSpriteResolver.SetCategoryAndLabel("LowerArm_Right", bossPartState.ToString());
        leftHandSpriteResolver.SetCategoryAndLabel("Hand_Left", bossPartState.ToString());
        rightHandSpriteResolver.SetCategoryAndLabel("Hand_Right", bossPartState.ToString());

    }
    private void Boss_OnBossHealthBelow25Perc(){
        if(BodyPartState < 4){
            BodyPartState = 4;
        }
        UpdateSprites(BodyPartState);
    }

    private void Boss_OnBossHealthBelow50Perc(){
        if(BodyPartState < 3){
            BodyPartState = 3;
        }
        UpdateSprites(BodyPartState);
    }

    private void Boss_OnBossHealthBelow75Perc(){
        if(BodyPartState < 2){
            BodyPartState = 2;
        }
        UpdateSprites(BodyPartState);
    }

    private void BossArms_OnHealthChanged(){
        if(HealthNormalized < 0.25f){
            BodyPartState = 4;
        }
        else if(HealthNormalized < 0.50f){
            BodyPartState = 3;
        }
        else if(HealthNormalized < 0.75f){
            BodyPartState = 2;
        }
        UpdateSprites(BodyPartState);
    }

    public int BodyPartState = 1;
    public bool IsThisPartAttacking = false;
    void Update(){
        Debug.Log("Boss Arms State:" + BodyPartState.ToString());
        Debug.Log("Boss Arms Health Normalized: " + HealthNormalized.ToString());
        if(Boss.PlayerIsInBossArea){
            bool bossHasFreeAttacks = Boss.NumberOfCurrentlyHappeningAttacks < Boss.MaximumConcurrentAttacks;
            if(!IsThisPartAttacking && bossHasFreeAttacks){
                int attackToUse = UnityEngine.Random.Range(1, BodyPartState+1); //+1 Because max is exclusive
                if(attackToUse == 1){
                    StartCoroutine(Attack_State1());
                }
                if(attackToUse == 2){
                    StartCoroutine(Attack_State2());
                }
                if(attackToUse == 3){
                    StartCoroutine(Attack_State3());
                }
                if(attackToUse == 4){
                    StartCoroutine(Attack_State4());
                }
            }
        }
    }
}

public partial class BossPart_Arms : IDamageable{
    public int MaxHealth;
    public float HealthNormalized{
        get{
            return (float)Health/(float)MaxHealth;
        }
    }
    public int Health { 
        get;  
        set;
    }
    public float InvincibilityDuration { get; set; }

    public Collider2D leftHandHitbox;
    public Collider2D rightHandHitbox;
    public IEnumerator MakeInvincible(){
        leftHandHitbox.enabled = false;
        rightHandHitbox.enabled = false;
        yield return new WaitForSeconds(InvincibilityDuration);
        leftHandHitbox.enabled = true;
        rightHandHitbox.enabled = true;
    }

    public void TakeDamage(int damage){
        StartCoroutine(MakeInvincible());
        this.Health = damage >= Health ? 0 : Health-damage;
        OnBossArmsHealthChanged?.Invoke();
    }
}

public partial class BossPart_Arms : IBossAttack{   
    public Transform attackTarget;
    public Transform armSwitchPoint;
    
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    public Transform leftHandRestPosition;
    public Transform rightHandRestPosition;
    public int damage;
    public float aimDuration = 3f; //Aim for 3 seconds
    public float moveSpeed = 30f;
    public float waitDuration = 0.5f;

    //State coefficients determine the speed
    //and damage that will be taken by the player
    public float state1Coefficient = 1f;
    public float state2Coefficient = 3f;
    public float state3Coefficient = 5f;
    public float state4Coefficient = 10f;


    public float coolDownInSecond = 3f;
    public IEnumerator Attack_State1(){
        IsThisPartAttacking = true;
        Boss.NumberOfCurrentlyHappeningAttacks++;
        //Calculate which hand to use
        Transform handToUse = attackTarget.position.x < armSwitchPoint.position.x ? leftHandTarget : rightHandTarget;
        Transform handToUseRestPosition = handToUse == leftHandTarget ? leftHandRestPosition : rightHandRestPosition;
        //Move hand up in the air
        yield return MoveHandUp(handToUse);
        //Try to aim towards the player
        yield return AimHandTowardsPlayer(handToUse, state1Coefficient);
        //Wait in the air for a small period of time
        yield return WaitInAir(handToUse, state1Coefficient);
        //Hit down
        yield return HitDown(handToUse, state1Coefficient);
        //Get stuck in the ground for a small time
        yield return new WaitForSeconds(1);
        //Move hand back to rest position
        yield return MoveHandToRestPosition(handToUse, handToUseRestPosition);
        
        Boss.NumberOfCurrentlyHappeningAttacks--;
        yield return new WaitForSeconds(coolDownInSecond);
        IsThisPartAttacking = false;
    }

    public IEnumerator Attack_State2(){
        IsThisPartAttacking = true;
        Boss.NumberOfCurrentlyHappeningAttacks++;
        //Calculate which hand to use
        Transform handToUse = attackTarget.position.x < armSwitchPoint.position.x ? leftHandTarget : rightHandTarget;
        Transform handToUseRestPosition = handToUse == leftHandTarget ? leftHandRestPosition : rightHandRestPosition;
        //Move hand up in the air
        yield return MoveHandUp(handToUse);
        //Try to aim towards the player
        yield return AimHandTowardsPlayer(handToUse, state2Coefficient);
        //Wait in the air for a small period of time
        yield return WaitInAir(handToUse, state2Coefficient);
        //Hit down
        yield return HitDown(handToUse, state2Coefficient);
        //Get stuck in the ground for a small time
        yield return new WaitForSeconds(1);
        //Move hand back to rest position
        yield return MoveHandToRestPosition(handToUse, handToUseRestPosition);
        
        Boss.NumberOfCurrentlyHappeningAttacks--;
        yield return new WaitForSeconds(coolDownInSecond);
        IsThisPartAttacking = false;
    }

    public IEnumerator Attack_State3(){
        IsThisPartAttacking = true;
        Boss.NumberOfCurrentlyHappeningAttacks++;
        //Calculate which hand to use
        Transform handToUse = attackTarget.position.x < armSwitchPoint.position.x ? leftHandTarget : rightHandTarget;
        Transform handToUseRestPosition = handToUse == leftHandTarget ? leftHandRestPosition : rightHandRestPosition;
        //Move hand up in the air
        yield return MoveHandUp(handToUse);
        //Try to aim towards the player
        yield return AimHandTowardsPlayer(handToUse, state3Coefficient);
        //Wait in the air for a small period of time
        yield return WaitInAir(handToUse, state3Coefficient);
        //Hit down
        yield return HitDown(handToUse, state3Coefficient);
        //Get stuck in the ground for a small time
        yield return new WaitForSeconds(1);
        //Move hand back to rest position
        yield return MoveHandToRestPosition(handToUse, handToUseRestPosition);

        Boss.NumberOfCurrentlyHappeningAttacks--;
        yield return new WaitForSeconds(coolDownInSecond);
        IsThisPartAttacking = false;
    }

    public IEnumerator Attack_State4(){
        IsThisPartAttacking = true;
        Boss.NumberOfCurrentlyHappeningAttacks++;
        //Calculate which hand to use
        Transform handToUse = attackTarget.position.x < armSwitchPoint.position.x ? leftHandTarget : rightHandTarget;
        Transform handToUseRestPosition = handToUse == leftHandTarget ? leftHandRestPosition : rightHandRestPosition;
        //Move hand up in the air
        yield return MoveHandUp(handToUse);
        //Try to aim towards the player
        yield return AimHandTowardsPlayer(handToUse, state4Coefficient);
        //Wait in the air for a small period of time
        yield return WaitInAir(handToUse, state4Coefficient);
        //Hit down
        yield return HitDown(handToUse, state4Coefficient);
        //Get stuck in the ground for a small time
        yield return new WaitForSeconds(1);
        //Move hand back to rest position
        yield return MoveHandToRestPosition(handToUse, handToUseRestPosition);
        
        Boss.NumberOfCurrentlyHappeningAttacks--;
        yield return new WaitForSeconds(coolDownInSecond);
        IsThisPartAttacking = false;
    }

    private IEnumerator MoveHandUp(Transform handToUse){
        Vector3 handAirPosition = new Vector3(handToUse.localPosition.x, -15, 0);
        while(handToUse.localPosition.y < handAirPosition.y){
            handToUse.localPosition = Vector3.MoveTowards(handToUse.localPosition, handAirPosition, moveSpeed*Time.deltaTime*2);
            yield return null;
        }
    }
    private IEnumerator AimHandTowardsPlayer(Transform handToUse, float stateCoefficient){
        float aimTimer = 0;
        while(aimTimer < aimDuration/stateCoefficient){
            Vector3 aimPosition = new Vector3(attackTarget.position.x, -15);
            handToUse.position = Vector3.MoveTowards(handToUse.position, aimPosition, moveSpeed*Time.deltaTime*stateCoefficient);
            handToUse.localPosition = new Vector3(handToUse.localPosition.x, -15);
            aimTimer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator WaitInAir(Transform handToUse, float stateCoefficient){
        float waitTimer = 0;
        while(waitTimer < waitDuration/stateCoefficient){
            waitTimer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator HitDown(Transform handToUse, float stateCoefficient){
        RaycastHit2D[] hits = Physics2D.BoxCastAll(origin: handToUse.position, size:Vector2.one*3, angle:0, direction:-Vector2.up, 1, LayerMask.GetMask("Ground"));
        while(hits.Length == 0){
            hits = Physics2D.BoxCastAll(origin: handToUse.position, size:Vector2.one*3, angle:0, direction:-Vector2.up, 1, LayerMask.GetMask("Ground", "PlayerTrigger"));
            handToUse.position = Vector3.MoveTowards(handToUse.position, new Vector3(handToUse.position.x, -50), moveSpeed*Time.deltaTime*stateCoefficient);

            foreach(RaycastHit2D hit in hits){
                if(hit.collider.tag == "Player"){
                    hit.collider.GetComponentInParent<IDamageable>().TakeDamage(damage * (int)stateCoefficient);
                }
            }
            yield return null;
        }
    }

    private IEnumerator MoveHandToRestPosition(Transform handToUse, Transform restPosition){
        float distanceOfHandToRestPosition = Mathf.Abs(Vector3.Distance(handToUse.position, restPosition.position));
        while(distanceOfHandToRestPosition > 0.1f){
            handToUse.position = Vector3.MoveTowards(handToUse.position, restPosition.position, moveSpeed*Time.deltaTime*2);
            distanceOfHandToRestPosition = Mathf.Abs(Vector3.Distance(handToUse.position, restPosition.position));
            yield return null;
        }
        handToUse.position = restPosition.position;
    }
}