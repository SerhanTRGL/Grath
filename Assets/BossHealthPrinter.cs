using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class BossHealthPrinter : MonoBehaviour{
    public TextMeshProUGUI valueText;
    public Boss boss;
    private void Update(){
        valueText.text = boss.Health.ToString();
    }
}
