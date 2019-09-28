using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellListController : MonoBehaviour
{

    public GameObject listItem;
    public Sprite elemental;
    public Sprite holy;
    public Sprite dark;

    string[] available_spells = {"one", "two", "expand dong", "the quick brown fox jumps over the lazy dog"};
    

    // Start is called before the first frame update
    void Start()
    {
      DrawSpellList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawSpellList()
    {
        foreach (Transform child in transform)
        {
          GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < available_spells.Length; i++)
        {
            GameObject obj = Instantiate(listItem, this.transform);
            obj.transform.localPosition = new Vector3(20, -20 - (20 + 100*obj.transform.localScale.y)*i, 0);
            obj.GetComponentInChildren<Text>().text = available_spells[i];
            obj.GetComponentsInChildren<Image>()[1].sprite = holy;
        }
    }
}
