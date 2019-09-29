using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // include so we can manipulate SceneManager

public class YetiIntroController: MonoBehaviour
{

    Animator anim;
    AudioSource audioSource;

    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
        StartCoroutine(fade());
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator fade() {
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("fadeInDone");
        audioSource.Play(); 
        yield return new WaitForSeconds(1.5f);
        anim.SetTrigger("idleDone");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("MainMenu");
    }
}
