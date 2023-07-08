using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Change OnPlayerDied to OnPlayerTeleportedToVillage
public class CharacterGenerator : MonoBehaviour{
    private CharacterNameGenerator m_characterNameGenerator = new CharacterNameGenerator();
    private CharacterVisualGenerator m_characterVisualGenerator;
    public Player player;
    public void GenerateNewCharacter(){
        m_characterVisualGenerator = GetComponent<CharacterVisualGenerator>();
        m_characterVisualGenerator.GeneratePlayerVisual();
        player.CharacterName = m_characterNameGenerator.GenerateNewCharacterName();
    }
}
