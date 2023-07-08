using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{
    public Player player;
    public List<GameObject> cameras = new List<GameObject>();
    public Transform playerSpawnPoint;
    private void Start() {
        Teleporter.OnPlayerTeleported += Teleporter_OnPlayerTeleported;
        Player.OnPlayerDied += Player_PlayerDied;
    }

    private void Player_PlayerDied(){
        StartCoroutine(WaitTimer());
    }

    private IEnumerator WaitTimer(){
        Boss.PlayerIsInBossArea = false;
        yield return new WaitForSeconds(1);
        player.transform.position = playerSpawnPoint.position;
        player.GetComponentInChildren<CharacterGenerator>().GenerateNewCharacter();
        Sword playerSword = player.Sword;
        player.Sword = null;
        Destroy(playerSword.gameObject);
        player.Health = player.MaxHealth;
        cameras[0].GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = player.transform;
        cameras[0].GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority++;
    }
    private void Teleporter_OnPlayerTeleported(){
        cameras[0].GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = null;
        cameras[0].GetComponent<Cinemachine.CinemachineVirtualCamera>().Priority--; 
    }

}
