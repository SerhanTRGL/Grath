using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ComboManager : MonoBehaviour{
    private float m_comboDuration;
    private float m_comboTimer;
    public float ComboTimer{
        get => m_comboTimer; 
        set => m_comboTimer = value > m_comboDuration ? m_comboDuration : value; //For resetting the timer, not too necessary
    }
    public float ComboDuration{
        get => m_comboDuration; 
        private set => m_comboDuration = value;
    }
    
    void Start(){
        m_comboTimer = 0;
        m_comboDuration = 1;
    }

    void Update(){
        m_comboTimer = m_comboTimer >= 0 ? m_comboTimer-Time.deltaTime : 0;
    }
}
