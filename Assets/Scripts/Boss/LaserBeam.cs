using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour{
    public GameObject closestGround;
    public EdgeCollider2D edgeCollider;
    public LineRenderer laserBeamLineRenderer;
    private List<GameObject> collidingGrounds; 
    public int damage;

    private void Awake() {
        collidingGrounds = new List<GameObject>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        laserBeamLineRenderer = GetComponent<LineRenderer>();
    }

    private void Update() {
        if(laserBeamLineRenderer.GetPosition(0) == laserBeamLineRenderer.GetPosition(1) && collidingGrounds.Count != 0){
            collidingGrounds.Clear();
            closestGround = null;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "ground" && !collidingGrounds.Contains(other.gameObject)){
            collidingGrounds.Add(other.gameObject);
            closestGround = CalculateClosestGround();
        }
        if(other.tag == "Player"){
            other.GetComponentInParent<IDamageable>().TakeDamage(damage);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.tag == "Player"){
            other.GetComponentInParent<IDamageable>().TakeDamage(damage);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(collidingGrounds.Contains(other.gameObject)){
            collidingGrounds.Remove(other.gameObject);
            closestGround = CalculateClosestGround();
        }
    }
    private GameObject CalculateClosestGround(){
        if(collidingGrounds.Count == 0){
            return null;
        }
        else{
            GameObject closestGround = collidingGrounds[0];
            float closestDistance = Mathf.Abs(Vector3.Distance(collidingGrounds[0].transform.position, laserBeamLineRenderer.GetPosition(0)));
            foreach(GameObject ground in collidingGrounds){
                float distance = Mathf.Abs(Vector3.Distance(ground.transform.position, laserBeamLineRenderer.GetPosition(0)));
                if(distance < closestDistance){
                    closestDistance = distance;
                    closestGround = ground;
                }
            }
            return closestGround;
        }
    }
    public void SetEdgeCollider(){
        List<Vector2> edges = new List<Vector2>();
        for(int point = 0; point < laserBeamLineRenderer.positionCount; point++){
            Vector3 lrPoint = laserBeamLineRenderer.GetPosition(point);
            edges.Add(new Vector2(lrPoint.x, lrPoint.y));
        }
        edgeCollider.SetPoints(edges);
    }
}
