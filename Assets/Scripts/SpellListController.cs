using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SpellListController : MonoBehaviour
{

    public GameObject listItem;
    public Sprite elemental;
    public Sprite holy;
    public Sprite dark;

    public Dictionary<string,string> spellDict = new Dictionary<string,string>();

    public int maxSpells;
    private int numSpells = 0;
    public List<string> availableSpells = new List<string>();

    // Start is called before the first frame update
    void Awake()
    {
        //spellDict.Add("potato", "elemental");
        //spellDict.Add("djas'sa's wrath", "dark");
        spellDict.Add("smite", "holy");
        spellDict.Add("save the amazon", "elemental");
    }
    void Start()
    {
        DrawSpellList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string CheckSpell(string spell)
    {
        if (spellDict.TryGetValue (spell, out string temp))
        {
            if (availableSpells.Contains(spell))
            {
                RemoveSpell (spell);
                return spell;
            }
            Debug.Log ("Player does not have scroll for spell");
        }
        return null;
    }

    public void DrawSpellList()
    {
        foreach (Transform child in transform)
        {
          GameObject.Destroy(child.gameObject);
        }
       
        for (int i = 0; i < numSpells; i++)
        {
            // Debug.Log(availableSpells[i]);
            string school = "";
            
            spellDict.TryGetValue(availableSpells[i], out school);
            GameObject obj = Instantiate(listItem, this.transform);
            obj.transform.localPosition = new Vector3(20, -20 - (20 + 100*obj.transform.localScale.y)*i, 0);
            obj.GetComponentInChildren<Text>().text = availableSpells[i];

            if (school == "holy")
              obj.GetComponentsInChildren<Image>()[1].sprite = holy;
            if (school == "dark")
              obj.GetComponentsInChildren<Image>()[1].sprite = dark;
            if (school == "elemental")
              obj.GetComponentsInChildren<Image>()[1].sprite = elemental;
        }
    }

    public bool AddSpell(string spellName)
    {
        if (numSpells < maxSpells)
        {
            availableSpells.Add(spellName);
            numSpells += 1;
            DrawSpellList();
            return true;
        }
        return false;
    }

    public bool RemoveSpell (string spellName)
    {
        if (numSpells > 0)
        {
            for (int i = 0; i < numSpells; i++)
            {
                if (spellName.Equals (availableSpells [i]))
                {
                    availableSpells.RemoveAt (i);
                    break;
                }
            }
            numSpells -= 1;
            DrawSpellList ();
            return true;
        }
        return false;
    }

    public string RandomScroll()
    {
        int i = Random.Range(0, spellDict.Count);
        return new List<string>(spellDict.Keys)[i];
    }
}
