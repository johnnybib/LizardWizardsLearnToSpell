using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace Photon.Pun.Demo.PunBasics
{
    public class TypingController : MonoBehaviour
    {
        public delegate void OnWordUpdate (string word);
        public static event OnWordUpdate WordUpdated;

        public delegate void OnSpellFire ();
        public static event OnSpellFire SpellFired;

        WizardPlayer player;
        private StringBuilder currWord;
        private string spell;
        private Dictionary<string, bool> spells = new Dictionary<string, bool> ();


        void Start()
        {
            currWord = new StringBuilder ();
            player = gameObject.GetComponentInParent<WizardPlayer> ();
        }


        void Update()
        {
            if (player.GetPhotonView().IsMine || player.GetTestingMode())
            {
                string input = Input.inputString;
                if (input.Equals (""))
                    return;
                if(Input.GetKey("up") || Input.GetKey("down") || Input.GetKey("left") || Input.GetKey("right"))
                    return;
                    
                UpdateWord (input [0]);
            }
        }

        void UpdateWord(char c) 
        {
            if (c == '\n' || c == '\r')
            {
                if (currWord.Length > 0)
                {
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
            if (player == null || player.GetSpellList () == null) 
            {
                Debug.LogError ("Cannot attempt spell, please add a WizardPlayer.cs script to your game object, and ensure that it has a SpellListController.");
                return;
            }
            // Send to List Controller
            spell = player.GetSpellList().CheckSpell(currWord.ToString().ToLower());
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
            if (player.GetSpellFunctions() == null) 
            {
                Debug.LogError ("Cannot fire spell, please add a WizardPlayer.cs script to your game object, and ensure that it has a SpellFunctions.");
                return;
            }
            player.GetSpellFunctions().FireSpell (spell);
            SpellFired ();
        }
    }
}

