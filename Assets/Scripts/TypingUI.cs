using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypingUI : MonoBehaviour
{
    [SerializeField]
    private Text wordText;

    private void OnEnable () 
    {
        TypingController.WordUpdated += UpdateUI;
    }

    private void OnDisable () 
    {
        TypingController.WordUpdated -= UpdateUI;
    }

    void UpdateUI(string word) 
    {
        wordText.text = word;
    }
}
