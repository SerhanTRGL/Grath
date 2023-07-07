using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BossPart_Arms : MonoBehaviour{
    private void Awake() {
        Health = MaxHealth;
    }
    // Update is called once per frame
    void Update(){
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            StartCoroutine(Attack_State1());
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            StartCoroutine(Attack_State2());
        }
        if(Input.GetKeyDown(KeyCode.Alpha3)){
            StartCoroutine(Attack_State3());
        }
        if(Input.GetKeyDown(KeyCode.Alpha4)){
            StartCoroutine(Attack_State4());
        }
    }
}

public partial class BossPart_Arms : IDamageable{
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

public partial class BossPart_Arms : IBossAttack{   
    public Transform attackTarget;
    public Transform armSwitchPoint;
    
    public Transform leftHandTarget;
    public Transform rightHandTarget;

    public Transform leftHandRestPosition;
    public Transform rightHandRestPosition;

    public float aimDuration = 3f; //Aim for 3 seconds
    public float moveSpeed = 30f;
    public float waitDuration = 0.5f;

    //State coefficients determine the speed
    //and damage that will be taken by the player
    public float state1Coefficient = 1f;
    public float state2Coefficient = 3f;
    public float state3Coefficient = 5f;
    public float state4Coefficient = 10f;

    public IEnumerator Attack_State1(){
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
    }

    public IEnumerator Attack_State2(){
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
    }

    public IEnumerator Attack_State3(){
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
    }

    public IEnumerator Attack_State4(){
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
            hits = Physics2D.BoxCastAll(origin: handToUse.position, size:Vector2.one*3, angle:0, direction:-Vector2.up, 1, LayerMask.GetMask("Ground"));
            handToUse.position = Vector3.MoveTowards(handToUse.position, new Vector3(handToUse.position.x, -50), moveSpeed*Time.deltaTime*stateCoefficient);
            yield return null;
        }
    }

    private IEnumerator MoveHandToRestPosition(Transform handToUse, Transform restPosition){
        float distanceOfHandToRestPosition = Mathf.Abs(Vector3.Distance(handToUse.position, restPosition.position));
        while(distanceOfHandToRestPosition > 0.1f){
            Debug.Log(distanceOfHandToRestPosition);
            handToUse.position = Vector3.MoveTowards(handToUse.position, restPosition.position, moveSpeed*Time.deltaTime*2);
            distanceOfHandToRestPosition = Mathf.Abs(Vector3.Distance(handToUse.position, restPosition.position));
            yield return null;
        }
        handToUse.position = restPosition.position;
    }
}