using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public partial class BossPart_Head : MonoBehaviour{
    public static event Action OnBossHeadHealthChanged;
    private void Awake() {
        Health = MaxHealth;
        OnBossHeadHealthChanged += BossHead_OnHealthChanged;
        InvincibilityDuration = 0.1f;
    }

    private void Start() {
        //Force increase state of the part depending on boss health
        Boss.OnBossHealthBelow75Perc += Boss_OnBossHealthBelow75Perc;
        Boss.OnBossHealthBelow50Perc += Boss_OnBossHealthBelow50Perc;
        Boss.OnBossHealthBelow25Perc += Boss_OnBossHealthBelow25Perc;
    }

    public SpriteResolver headSpriteResolver;

    private void UpdateSprites(int bossPartState){
        headSpriteResolver.SetCategoryAndLabel("Head", bossPartState.ToString());
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

    private void Boss_OnBossHealthBelow75Perc()
    {
        if(BodyPartState < 2){
            BodyPartState = 2;
        }
        UpdateSprites(BodyPartState);
    }

    private void BossHead_OnHealthChanged(){
        if(HealthNormalized < 0.25f){
            BodyPartState = 4;
        }
        else if(HealthNormalized < 0.50f){
            BodyPartState = 3;
        }
        else if(HealthNormalized < 0.75){
            BodyPartState = 2;
        }
        UpdateSprites(BodyPartState);
    }

    public int BodyPartState = 1;
    public bool IsThisPartAttacking = false;
    void Update(){
        Debug.Log("Boss Head State:" + BodyPartState.ToString());
        Debug.Log("Boss Head Health Normalized: " + HealthNormalized.ToString());
        if(BodyPartState == 4){
            StartCoroutine(Attack_State4());
        }

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
            }
        }
    }
}

public partial class BossPart_Head : IDamageable{
    [Header("Part Attributes")]
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
    public Collider2D headHitbox;
    public IEnumerator MakeInvincible(){
        headHitbox.enabled = false;
        yield return new WaitForSeconds(InvincibilityDuration);
        headHitbox.enabled = true;
    }

    public void TakeDamage(int damage){
        StartCoroutine(MakeInvincible());
        this.Health = damage >= Health ? 0 : Health-damage;
        OnBossHeadHealthChanged?.Invoke();
    }
}

public partial class BossPart_Head : IBossAttack{
    [Header("General")]
    public Transform attackTarget;
    public GameObject fireballPrefab;
    public float coolDownInSecond = 2.5f;

    [Header("State 1")]
    public Transform firePoint;
    public float fireballMoveSpeed = 20f;
    public IEnumerator Attack_State1(){
        IsThisPartAttacking = true;
        Boss.NumberOfCurrentlyHappeningAttacks++;

        Fireball fireball = Instantiate(fireballPrefab, firePoint).GetComponent<Fireball>();
        fireball.sticksToGround = false;

        Vector3 hitTarget = attackTarget.position;
        Vector3 moveDir = (hitTarget-firePoint.position).normalized;
        float timeOutTimer = 0;
        float timeOutDuration = 10f; //If nothing is hit in 10 seconds, destroy object
        while(fireball != null && timeOutTimer<timeOutDuration){
            fireball.rb.velocity = moveDir * fireballMoveSpeed;
            yield return null;
        }
        if(fireball != null){
            Destroy(fireball.gameObject);
        }
        
        Boss.NumberOfCurrentlyHappeningAttacks--;
        yield return new WaitForSeconds(coolDownInSecond);
        IsThisPartAttacking = false;
    }

    
    [Header("State 2")]
    public LineRenderer laserBeamLineRenderer;
    public LaserBeam laserBeam;
    public float attackDuration = 5f;
    public float laserFollowSpeed = 10f;
    
    public IEnumerator Attack_State2(){
        IsThisPartAttacking = true;
        Boss.NumberOfCurrentlyHappeningAttacks++;

        float attackTimer = 0f;

        laserBeamLineRenderer.SetPosition(0, firePoint.position);
        laserBeamLineRenderer.SetPosition(1, firePoint.position);
        
        //First make sure line renderer hits the ground while aiming towards the player
        while(laserBeam.closestGround == null){
            Vector3 newPosition = Vector3.MoveTowards(laserBeamLineRenderer.GetPosition(1), new Vector3(firePoint.position.x, -200), 0.5f);
            laserBeamLineRenderer.SetPosition(1, newPosition);
            laserBeam.SetEdgeCollider();
            yield return null;
        }

        //Second, aim towards the player for the attack duration, while only changing the X position of the tip of the line
        Vector3 currentTargetPosition = laserBeamLineRenderer.GetPosition(1);
        Debug.Log("Aiming towards player?");
        while(attackTimer < attackDuration){
            Vector3 direction = currentTargetPosition - laserBeamLineRenderer.GetPosition(0);
            RaycastHit2D hit = Physics2D.Raycast(laserBeamLineRenderer.GetPosition(0), direction, Mathf.Infinity, LayerMask.GetMask("Ground"));

            Vector3 endPosition = hit.point;

            laserBeamLineRenderer.SetPosition(1, endPosition);
            laserBeam.SetEdgeCollider();
            attackTimer += Time.deltaTime;
            currentTargetPosition = Vector3.MoveTowards(currentTargetPosition, attackTarget.position, laserFollowSpeed*Time.deltaTime);
            yield return null;
        }

        laserBeamLineRenderer.SetPosition(1, firePoint.position);
        laserBeam.SetEdgeCollider();
        
        Boss.NumberOfCurrentlyHappeningAttacks--;
        yield return new WaitForSeconds(coolDownInSecond);
        IsThisPartAttacking = false;
    }

    [Header("State 3")]
    public int fireballAmount = 10;
    public float fireballShootPerSecond = 5f;
    public float flamethrowerFireballSpeed = 20f;
    public float fireLivesOnGroundSeconds = 5;
    public IEnumerator Attack_State3(){
        IsThisPartAttacking = true;
        Boss.NumberOfCurrentlyHappeningAttacks++;

        //Fire towards player
        for(int i = 0; i<fireballAmount; i++){
            Fireball fireball = Instantiate(fireballPrefab, firePoint).GetComponent<Fireball>();
            fireball.sticksToGround = true;
            fireball.groundStickDuration = fireLivesOnGroundSeconds;

            Vector3 hitTarget = attackTarget.position;
            Vector3 moveDir = (hitTarget-firePoint.position).normalized;
            fireball.rb.velocity = moveDir * flamethrowerFireballSpeed;
            yield return new WaitForSeconds(1/fireballShootPerSecond);
        }
        
        Boss.NumberOfCurrentlyHappeningAttacks--;
        yield return new WaitForSeconds(coolDownInSecond);
        IsThisPartAttacking = false;
    }

    [Header("State 4")]
    public float fireBurstDuration = 5f;
    public float coolDownDuration = 3f;
    public ParticleSystem backgroundFlameParticles;
    public ParticleSystem foregroundFlameParticles;
    public BurningHead burningHead;
    public IEnumerator Attack_State4() {
        while(true){
            burningHead.headBurning = true;
            var emission = backgroundFlameParticles.emission;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(500);
            emission = foregroundFlameParticles.emission;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(100);
            yield return new WaitForSeconds(fireBurstDuration);

            burningHead.headBurning = false;
            emission = backgroundFlameParticles.emission;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(50);
            emission = foregroundFlameParticles.emission;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(10);
            yield return new WaitForSeconds(coolDownDuration);
        }
    }
}
