﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public BoardManager boardScript;
    
    void Awake()
    {
        boardScript = GetComponent<BoardManager>();
        InitGame(); 
    }

     
    void InitGame()
    {
        boardScript.SetupScene();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
