using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScrollController : MonoBehaviour
{
    public string spellName;
    public string spellSchool;
    private string scrollID;
    void Start()
    {
        //Change image based on spellSchool
        scrollID = System.Guid.NewGuid().ToString("N");
    }

    void Update()
    {

    }

    public string GetScrollID()
    {
        return scrollID;
    }
                        
}
