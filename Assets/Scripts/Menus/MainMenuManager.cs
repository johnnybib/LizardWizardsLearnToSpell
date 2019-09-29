using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    [SerializeField]
    private string sceneToLoad;

    // Called by start game button
    public void StartGame () {
        StartCoroutine(StartGameDelay());  
    }

    // Called by quit game button
    public void QuitGame () {
        Application.Quit();
    }

    IEnumerator StartGameDelay() {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneToLoad);
    }
}
