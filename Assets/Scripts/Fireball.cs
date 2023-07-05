using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour{
    public int damage;
    public Rigidbody2D rb;
    public bool sticksToGround;
    public float groundStickDuration;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private float aliveTimer = 0;
    private void Update() {
        aliveTimer += Time.deltaTime;
        if(aliveTimer > 20){
            StopAllCoroutines();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Fireball collision");
        if(other.tag == "Player"){
            other.GetComponent<IDamageable>().TakeDamage(damage);
        }
        if(this.sticksToGround && other.tag == "ground"){
            rb.velocity = Vector3.zero;
            aliveTimer = 0;
            StartCoroutine(WaitOnGround());
        }
        if(!this.sticksToGround){
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D other) {
        if(other.tag == "Player" && this.sticksToGround){
            other.GetComponent<IDamageable>().TakeDamage(damage/2);
        }
    }
    IEnumerator WaitOnGround(){
        yield return new WaitForSeconds(groundStickDuration);
        Destroy(this.gameObject);
    }
}
