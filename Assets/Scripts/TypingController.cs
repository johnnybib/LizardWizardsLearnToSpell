using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TypingController : MonoBehaviour
{
    public delegate void OnWordUpdate (string word);
    public static event OnWordUpdate WordUpdated;
    private StringBuilder currWord;
    private string spell;
    private Dictionary<string, bool> spells = new Dictionary<string, bool> ();


    void Start()
    {
        
        currWord = new StringBuilder ();
        // TODO update spell dictionary to read from somewhere else
        spells.Add ("EXPAND DONG", true);
    }


    void Update()
    {
        string input = Input.inputString;
        if (input.Equals (""))
            return;
        //Debug.Log ("Input is: " + input);
        UpdateWord (input [0]);

    }

    void UpdateWord(char c) 
    {
        // if backspace
        if (c == '\b' && currWord.Length > 0) 
        {
            // Remove last letter.
            currWord.Remove (currWord.Length - 1, 1); 
        }
        else 
        {
            currWord.Append (c);
        }
        WordUpdated (currWord.ToString ());
        //Debug.Log ("currword is " + currWord);

        spell = CheckDictionary ();

        if (spell != null) 
        {
            FireSpell (spell);
        }
    }

    private string CheckDictionary() 
    {
        string spell = currWord.ToString ();
        if (spells.TryGetValue (spell, out bool temp))
            return spell;
        return null;
    }

    private void FireSpell(string spell) 
    {
        Debug.Log ("FIRING SPELL: " + spell);
    }
}
