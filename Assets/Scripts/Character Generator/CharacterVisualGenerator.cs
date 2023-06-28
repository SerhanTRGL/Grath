using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class CharacterVisualGenerator:MonoBehaviour{
    private List<String> m_labels = new List<string>(){"Entry", "Entry_0", "Entry_1", "Entry_2"};

    public SpriteResolver m_upperFrontLeg;
    public SpriteResolver m_upperFrontArm;
    public SpriteResolver m_lowerFrontLeg;
    public SpriteResolver m_lowerFrontArm;
    public SpriteResolver m_head;
    public SpriteResolver m_torso;
    public SpriteResolver m_upperBackLeg;
    public SpriteResolver m_upperBackArm;
    public SpriteResolver m_lowerBackLeg;
    public SpriteResolver m_lowerBackArm;
    public void GeneratePlayerVisual(){
        System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
        
        m_upperFrontLeg.SetCategoryAndLabel("Front Leg Upper", m_labels[rnd.Next(m_labels.Count)]);
        m_upperFrontArm.SetCategoryAndLabel("Front Arm Upper", m_labels[rnd.Next(m_labels.Count)]);
        m_lowerFrontLeg.SetCategoryAndLabel("Front Leg Lower", m_labels[rnd.Next(m_labels.Count)]);
        m_lowerFrontArm.SetCategoryAndLabel("Front Arm Lower", m_labels[rnd.Next(m_labels.Count)]);
        m_head.SetCategoryAndLabel("Head", m_labels[rnd.Next(m_labels.Count)]);
        m_torso.SetCategoryAndLabel("Torso", m_labels[rnd.Next(m_labels.Count)]);
        m_upperBackLeg.SetCategoryAndLabel("Back Leg Upper", m_labels[rnd.Next(m_labels.Count)]);
        m_upperBackArm.SetCategoryAndLabel("Back Arm Upper", m_labels[rnd.Next(m_labels.Count)]);
        m_lowerBackLeg.SetCategoryAndLabel("Back Leg Lower", m_labels[rnd.Next(m_labels.Count)]);
        m_lowerBackArm.SetCategoryAndLabel("Back Arm Lower", m_labels[rnd.Next(m_labels.Count)]);

    }
}
