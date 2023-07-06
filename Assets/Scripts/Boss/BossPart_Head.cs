using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BossPart_Head : MonoBehaviour{
    
    private void Awake() {
        Health = MaxHealth;
    }
    // Update is called once per frame
    void Update(){
        
    }
}

public partial class BossPart_Head : IDamageable{
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

public partial class BossPart_Head : IBossAttack{
    
    public Transform AttackTarget { get => attackTarget; set => attackTarget = value; }
    
    [Header("General")]
    public Transform attackTarget;
    public GameObject fireballPrefab;

    [Header("State 1")]
    
    public Transform firePoint;
    public float fireballMoveSpeed = 20f;
    public IEnumerator Attack_State1(){
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
    }

    
    [Header("State 2")]
    public LineRenderer laserBeamLineRenderer;
    public LaserBeam laserBeam;
    public float attackDuration = 5f;
    public float laserFollowSpeed = 10f;
    private float attackTimer = 0f;
    public IEnumerator Attack_State2(){
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
    }

    [Header("State 3")]
    public int fireballAmount = 10;
    public float fireballShootPerSecond = 5f;
    public float flamethrowerFireballSpeed = 20f;
    public float fireLivesOnGroundSeconds = 5;
    public IEnumerator Attack_State3(){
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
