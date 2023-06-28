using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour{
    private CharacterNameGenerator m_characterNameGenerator = new CharacterNameGenerator();
    private CharacterVisualGenerator m_characterVisualGenerator;
    public Player player;
    // Update is called once per frame
    void Start(){
        m_characterVisualGenerator = GetComponent<CharacterVisualGenerator>();
    }
    void Update(){
        string fullName = "";
        if(Input.GetKeyDown(KeyCode.E) && fullName != null){
            m_characterVisualGenerator.GeneratePlayerVisual();
            fullName = m_characterNameGenerator.GenerateNewCharacterName();
            Debug.Log(fullName + " Number of names generated: " + m_characterNameGenerator.GeneratedNameCount);
            player.PlayerName = fullName;
        }
    }
}
