using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSpawner : MonoBehaviour{
    // Start is called before the first frame update
    public GameObject[] swordPrefabs;

    void Start(){
        Player.OnPlayerDied += Player_OnPlayerDied;
    }

    private void Player_OnPlayerDied(){
        int index = UnityEngine.Random.Range(0, swordPrefabs.Length);
        GameObject swordToUse = swordPrefabs[index];
        GameObject newSword = Instantiate(swordToUse);
        newSword.transform.position = this.transform.position;
    }
}
