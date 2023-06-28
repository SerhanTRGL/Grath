using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangePlayerName : MonoBehaviour{
    public RectTransform canvasTransform;
    public TextMeshProUGUI playerName;
    public Player player;
    void Update(){
        playerName.text = player.PlayerName;
        if(player.transform.rotation.eulerAngles.y == -180){
            canvasTransform.rotation = Quaternion.Euler(canvasTransform.rotation.eulerAngles.x, -180, canvasTransform.rotation.eulerAngles.z);
        }
        else{
            canvasTransform.rotation = Quaternion.Euler(canvasTransform.rotation.eulerAngles.x, 0, canvasTransform.rotation.eulerAngles.z);
        }

    }
}
