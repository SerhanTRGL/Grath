using System.Collections;
using System.Collections.Generic;
using System;

public class CharacterNameGenerator{
    private int MAX_NUM_OF_POSSIBLE_FULL_NAMES = m_firstNames.Count * m_lastNames.Count;
    private static List<string> m_firstNames = new List<string>(){
        "Luthera", "Eithne", "Colette", "Sabina", "Groa", "Petra", "Amelia", 
        "Edme", "Acquilina", "Philomena", "Berenice", "Adelaide", "Eleanor", 
        "Olive", "Desislava", "Mathilda", "Clotilda", "Euphemia", "Ursula", 
        "Orla", "Isolde", "Erika", "Avice", "Brunhilda", "Sigrid", "Regina", 
        "Brenna", "Thomasina", "Eira", "Zelda", "Maude", "Genevieve", "Bodil", 
        "Beatriz", "Hildegund", "Martine", "Winifred", "Mirabel", "Odelgarde", 
        "Ella", "Gregoria", "Emmalina", "Hawise", "Dominic", "Kenric", "Gregory", 
        "Burchard", "Lancaster", "Benedict", "Finnian", "Björn", "Randolf", 
        "Daegal", "Egil", "Dustin", "Orvyn", "Hamlin", "Odel", "Gerold", "Badulf", 
        "Haywood", "Florian", "Bahram", "Arthur", "Cassian", "Arne", "Cathasach", 
        "Jeremiah", "Bartholomew", "Galileo", "Wymond", "Cyprian", "Gage", "Leif", 
        "Theodoric", "Zemislav", "Aldous", "Archibald", "Neville", "Henry", "Conrad", 
        "Ellis", "Merlin", "Gandalf", "Godfrey", "Bard", "Charibert", "Bertram", 
        "Torsten", "Wilkin", "Godwin", "Serhan", "Orkan"
    };
    private static List<string> m_lastNames = new List<string>(){
        "Ace", "Acey", "Adlington", "Adrienne", "Agincourt", "Aimar", "Allaire", 
        "Allsop", "Appel", "Baker", "Bamford", "Barnard", "Barnewall", "Beaulieu", 
        "Beaumont", "Beckett", "Berryann", "Blakewood", "Blanning", "Bliss", 
        "Binns", "Bisby", "Bowe", "Brentford", "Brewer", "Browne", "Burdick", 
        "Carey", "Carter", "Ead", "Easton", "Edison", "Elias", "Elwood", "Emory", 
        "English", "Erickson", "Farmer", "Fischer", "Fish", "Foster", "Fuller", 
        "Geoffrey", "Gerard", "Godfrey", "Graves", "Greaves", "Griffin", "Griswold", 
        "Grove", "Grover", "Gwydion", "Hackett", "Harcourt", "Hawkins", "Hawthorne", 
        "Haynes", "Hayward", "Hazel", "Head", "Helm", "Hendrey", "Hill", "Holley", 
        "Holloway", "Holt", "Homer", "Hood", "Hooke", "Hope", "Howe", "Hughes", "Hume", 
        "Jester", "Kaye", "Keats", "Kemp", "Kerr", "Kirk", "Knapp", "Knott", "Knowles", 
        "Laidler", "Lamb", "Lambert", "Lancaster", "Lancelot", "Langdon", "Lawless", "Lee", 
        "Leigh", "Mabey", "Mallory", "Marsh", "Marshall", "May", "Medley", "Mercer", "Meredith", 
        "Middleton", "Miller", "Montgomery", "Morrison", "Mortimer", "Mowbray", "Napier", 
        "Nesmith", "Neville", "Newby", "Newcomb", "Norton", "Oakey", "Paige", "Paine", "Park", 
        "Pike", "Pitt", "Prescott", "Purvis", "Quinnell", "Raleigh", "Reynold", "Ridge", 
        "Roger", "Rolfe", "Roth", "Rountree", "Rowan", "Ryder", "Scroggs", "Seller", "Shepherd", 
        "Shore", "Slater", "Smith", "Sommers", "Steele", "Stoddard", "Swift", "Sykes", "Taylor", 
        "Thorn", "Tilly", "Tull", "Turner", "Tyler", "Underwood", "Vaughan", "Vernon", "Webber", 
        "West", "Whitney", "Wilde", "Wilkins", "Williamson", "Willoughby", "Witherspoon", "Wood",
        "Wright", "Yorke", "Zimmerman", "Atabey", "Turgul"
    };
    private List<string> m_generatedNames = new List<string>();
    public int GeneratedNameCount {
        get{
            return m_generatedNames.Count;
        }
        private set{}
    }
    public List<string> UsedNames{
        get{
            return m_generatedNames;
        } 
        private set{}
    }

    public string GenerateNewCharacterName(){
        /*
        **Return null if all possible names are generated
        */
        if(m_generatedNames.Count == MAX_NUM_OF_POSSIBLE_FULL_NAMES){
            return null;
        }

        Random rnd = new Random(Guid.NewGuid().GetHashCode()); //Random seed
       
        bool foundUniqueName = false;
        
        int firstNameIndex;
        int lastNameIndex;

        string firstName = "";
        string lastName = "";
        string fullName = "";

        /*
        ** Generate a new name until a combination that was not used
        ** is found. A first name or a last name can be in used state on their own.
        ** for unique full name, only 
        */
        while(!foundUniqueName){
            firstNameIndex = rnd.Next(m_firstNames.Count);
            lastNameIndex = rnd.Next(m_lastNames.Count);

            firstName = m_firstNames[firstNameIndex];
            lastName = m_lastNames[lastNameIndex];

            fullName = firstName + " " + lastName;
            foundUniqueName = !(m_generatedNames.Contains(fullName));
        }

        m_generatedNames.Add(fullName);

        return fullName;
    }

}
