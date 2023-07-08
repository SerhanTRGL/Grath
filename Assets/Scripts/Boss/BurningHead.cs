using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningHead : MonoBehaviour{
    public bool headBurning;
    public int damage;
    private void OnTriggerStay2D(Collider2D other) {
        if(other.tag == "Player" && this.headBurning){
            other.gameObject.GetComponentInParent<IDamageable>().TakeDamage(damage);
        }
    }
}
