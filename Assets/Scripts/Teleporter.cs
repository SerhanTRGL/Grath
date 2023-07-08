using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour{
    public Transform teleportPoint;
    
    public static event Action OnPlayerTeleported;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            OnPlayerTeleported?.Invoke();
            other.transform.parent.position = teleportPoint.position;
        }
    }

}
