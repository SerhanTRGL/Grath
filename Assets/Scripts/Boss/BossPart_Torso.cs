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
            StartCoroutine(Attack_State4());
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
    [Header("General")]
    public Transform attackTarget;
    public GameObject fireballPrefab;
    public Transform firePoint;
    public int numberOfFireballsInCircle;
    public float fireballMoveSpeed;
    
    [Header("State 1")]
    public float state1CooldownTime;
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
        yield return new WaitForSeconds(state1CooldownTime);
    }

    [Header("State 2")]
    public float numberOfWaves;
    public float timeBetweenCirclesInSecond;
    public float state2CooldownTime;
    public IEnumerator Attack_State2(){
        for(int i = 0; i < numberOfWaves; i++){
            for(int j = 0; j < numberOfFireballsInCircle; j++){
                float radians = (2 * Mathf.PI / numberOfFireballsInCircle) * j + (2 * Mathf.PI / numberOfWaves) * i;

                float verticalDir = Mathf.Sin(radians);
                float horizontalDir = Mathf.Cos(radians);

                Vector3 moveDir = new Vector3(horizontalDir, verticalDir);
                Fireball fireball = Instantiate(fireballPrefab, firePoint).GetComponent<Fireball>();
                fireball.sticksToGround = false;

                fireball.rb.velocity = moveDir * fireballMoveSpeed;
            }
            yield return new WaitForSeconds(timeBetweenCirclesInSecond);
        }
        yield return new WaitForSeconds(state2CooldownTime);
    }

    [Header("State 3")]
    public LaserBeam[] left_laserBeams;
    public LaserBeam[] right_laserBeams;
    public LaserBeam middle_laserBeam;
    public Transform laserBeamStartPoint;
    public Transform laserBeamEndPoint;
    public float distanceOffset;
    public float timeBetweenBeamsInSecond;
    public float timeBetweenStartPointToEndPointInSecond;
    public IEnumerator Attack_State3(){
        float initialXPosition = attackTarget.position.x;
        float initialYPosition = laserBeamStartPoint.position.y;
        float finalYPosition = laserBeamEndPoint.position.y;

        middle_laserBeam.laserBeamLineRenderer.SetPosition(0, new Vector3(initialXPosition, initialYPosition));
        middle_laserBeam.laserBeamLineRenderer.SetPosition(1, new Vector3(initialXPosition, initialYPosition));
        float elapsedTime = 0;

        while(elapsedTime < timeBetweenStartPointToEndPointInSecond){
            Vector3 positionToGo = Vector3.Lerp(middle_laserBeam.laserBeamLineRenderer.GetPosition(0), new Vector3(initialXPosition, finalYPosition), elapsedTime/timeBetweenStartPointToEndPointInSecond);
            middle_laserBeam.laserBeamLineRenderer.SetPosition(1, positionToGo);
            middle_laserBeam.SetEdgeCollider();
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        middle_laserBeam.laserBeamLineRenderer.SetPosition(1, new Vector3(initialXPosition, finalYPosition));
        middle_laserBeam.SetEdgeCollider();

        yield return new WaitForSeconds(timeBetweenBeamsInSecond);
        middle_laserBeam.laserBeamLineRenderer.SetPosition(0, Vector3.zero);
        middle_laserBeam.laserBeamLineRenderer.SetPosition(1, Vector3.zero);
        middle_laserBeam.SetEdgeCollider();

        for(int i = 0; i < left_laserBeams.Length; i++){
            elapsedTime = 0;
            left_laserBeams[i].laserBeamLineRenderer.SetPosition(0, new Vector3(initialXPosition - distanceOffset*(i+1), initialYPosition));
            left_laserBeams[i].laserBeamLineRenderer.SetPosition(1, new Vector3(initialXPosition - distanceOffset*(i+1), initialYPosition));
            
            right_laserBeams[i].laserBeamLineRenderer.SetPosition(0, new Vector3(initialXPosition + distanceOffset*(i+1), initialYPosition));
            right_laserBeams[i].laserBeamLineRenderer.SetPosition(1, new Vector3(initialXPosition + distanceOffset*(i+1), initialYPosition));

            while(elapsedTime < timeBetweenStartPointToEndPointInSecond){
                Vector3 positionToGo_leftBeam = Vector3.Lerp(left_laserBeams[i].laserBeamLineRenderer.GetPosition(0), new Vector3(initialXPosition - distanceOffset*(i+1), finalYPosition), elapsedTime/timeBetweenStartPointToEndPointInSecond);
                Vector3 positionToGo_rightBeam = Vector3.Lerp(right_laserBeams[i].laserBeamLineRenderer.GetPosition(0), new Vector3(initialXPosition + distanceOffset*(i+1), finalYPosition), elapsedTime/timeBetweenStartPointToEndPointInSecond);
                
                left_laserBeams[i].laserBeamLineRenderer.SetPosition(1, positionToGo_leftBeam);
                right_laserBeams[i].laserBeamLineRenderer.SetPosition(1, positionToGo_rightBeam);

                left_laserBeams[i].SetEdgeCollider();
                right_laserBeams[i].SetEdgeCollider();
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            left_laserBeams[i].laserBeamLineRenderer.SetPosition(1, new Vector3(initialXPosition - distanceOffset*(i+1), finalYPosition));
            left_laserBeams[i].SetEdgeCollider();

            right_laserBeams[i].laserBeamLineRenderer.SetPosition(1, new Vector3(initialXPosition + distanceOffset*(i+1), finalYPosition));
            right_laserBeams[i].SetEdgeCollider();
            yield return new WaitForSeconds(timeBetweenBeamsInSecond);

            left_laserBeams[i].laserBeamLineRenderer.SetPosition(0, Vector3.zero);
            left_laserBeams[i].laserBeamLineRenderer.SetPosition(1, Vector3.zero);
            left_laserBeams[i].SetEdgeCollider();

            right_laserBeams[i].laserBeamLineRenderer.SetPosition(0, Vector3.zero);
            right_laserBeams[i].laserBeamLineRenderer.SetPosition(1, Vector3.zero);
            right_laserBeams[i].SetEdgeCollider();
        }
    }

    [Header("State 4")]
    public GameObject laserBeamWithEnergyBallPrefab;
    public int numberOfObjectsToSpawn;
    public float distanceFromEdges;
    public float energyBallGrowTime;
    public float energyBallAttackDuration;
    public float energyBallLaserWaitDuration;
    public float laserLength = 10;
    public IEnumerator Attack_State4(){
        List<GameObject> objects = new List<GameObject>();
        List<Vector3> attackDirections = new List<Vector3>();

        for(int i = 0; i < numberOfObjectsToSpawn; i++){
            objects.Add(Instantiate(laserBeamWithEnergyBallPrefab));
            objects[i].transform.position = GetRandomSpawnPosition();
            objects[i].GetComponentInChildren<LaserBeam>().laserBeamLineRenderer.SetPosition(0, objects[i].transform.position);
            objects[i].GetComponentInChildren<LaserBeam>().laserBeamLineRenderer.SetPosition(1, objects[i].transform.position);

            objects[i].GetComponentInChildren<EdgeCollider2D>().offset = -objects[i].GetComponentInChildren<LaserBeam>().laserBeamLineRenderer.GetPosition(0);
            //Attack direction for each energy ball
            attackDirections.Add(attackTarget.position - objects[i].transform.position);
        }

        //Start growing the objects
        float energyBallGrowTimer = 0;
        while(energyBallGrowTimer <= energyBallGrowTime){
            foreach(GameObject g in objects){
                g.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, energyBallGrowTimer/energyBallGrowTime);
            }
            energyBallGrowTimer += Time.deltaTime;
            yield return null;
        }

        //Make sure the objects have a size of 1 at the end
        foreach(GameObject g in objects){
            g.transform.localScale = Vector3.one;
        }
        yield return null;

        //Make all energy balls attack
        float energyBallAttackTimer = 0;
        while(energyBallAttackTimer < energyBallAttackDuration){
            for(int i = 0; i < objects.Count; i++){
                LaserBeam laserBeam = objects[i].GetComponentInChildren<LaserBeam>();
                Vector3 movePosition = attackDirections[i] * laserLength;
                Vector3 newPosition = Vector3.Lerp(laserBeam.laserBeamLineRenderer.GetPosition(1), movePosition, energyBallAttackTimer/energyBallAttackDuration);
                laserBeam.laserBeamLineRenderer.SetPosition(1, newPosition);
                laserBeam.SetEdgeCollider();
            }
            energyBallAttackTimer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(energyBallLaserWaitDuration);
        foreach(GameObject g in objects){
            Destroy(g);
        }
        objects.Clear();
        attackDirections.Clear();

    }
    
    private Vector3 GetRandomSpawnPosition(){
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = Camera.main.aspect * cameraHeight;

        //Calculate adjusted bounds;
        float adjustedCameraHeight = cameraHeight - distanceFromEdges;
        float adjustedCameraWidth = cameraWidth - distanceFromEdges;

        float randomX = Random.Range(-adjustedCameraWidth, adjustedCameraWidth);
        float randomY = Random.Range(-adjustedCameraHeight, adjustedCameraHeight);

        Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(
            (randomX + adjustedCameraWidth) / (2f * adjustedCameraWidth),
            (randomY + adjustedCameraHeight) / (2f * adjustedCameraHeight),
            Camera.main.nearClipPlane));
        spawnPosition.z = 0;
        return spawnPosition;
    }
}
