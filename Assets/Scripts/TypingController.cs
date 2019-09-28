using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TypingController : MonoBehaviour
{
    public delegate void OnWordUpdate (string word);
    public static event OnWordUpdate WordUpdated;
    public CameraController cameraController;
    private StringBuilder currWord;
    private string spell;
    private Dictionary<string, bool> spells = new Dictionary<string, bool> ();


    void Start()
    {
        
        currWord = new StringBuilder ();
        // TODO update spell dictionary to read from somewhere else
        spells.Add ("expand dong", true);
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
        if (c == '\n' || c == '\r')
        {
            if (currWord.Length > 0)
            {
                cameraController.StartZoom();
                AttemptSpell();
            }
        }
        else
        {
            // if backspace, remove last letter
            if (c == '\b' && currWord.Length > 0) 
                currWord.Remove (currWord.Length - 1, 1); 
            else
                currWord.Append (c);
        }
        WordUpdated (currWord.ToString ());
    }

    private void AttemptSpell()
    {
        spell = CheckDictionary();
        if (spell != null) 
            FireSpell (spell);
        currWord = new StringBuilder();
    }

    private string CheckDictionary() 
    {
        string spell = currWord.ToString().ToLower();
        if (spells.TryGetValue (spell, out bool temp))
            return spell;
        return null;
    }

    private void FireSpell(string spell) 
    {
        Debug.Log ("FIRING SPELL: " + spell);
    }
}
