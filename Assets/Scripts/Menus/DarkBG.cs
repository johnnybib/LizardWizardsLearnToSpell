using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBG : MonoBehaviour {
    Animator anim;

    // Use this for initialization
    void Start() {
        anim = GetComponent<Animator>();
    }

    // Called by start game button
    public void StartGame() {
        anim.SetTrigger("fadeOut");
    }
}
