using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource levelMusic;
    [SerializeField]
    private AudioSource lowHealthMusic;
    [SerializeField]
    private AudioSource powerupMusic;
    private float scaleSpeed = 0.3f;

    private AudioSource asFrom;
    private AudioSource asTo;

    private bool isTransitioning;

    private void Awake () {
        levelMusic.volume = 1.0f;
        lowHealthMusic.volume = 0.0f;
        powerupMusic.volume = 0.0f;
    }

    void Start () {
        isTransitioning = false;
        asFrom = lowHealthMusic;
        asTo = levelMusic;
    }


    void Update () {
        if (asFrom.volume <= 0.0f && asTo.volume >= 1.0f)
            isTransitioning = false;
        if (!isTransitioning)
            return;
        asFrom.volume -= scaleSpeed * Time.deltaTime;
        asTo.volume += scaleSpeed * Time.deltaTime;

    }

    // On low health
    // isTransitioning = true;
    // asFrom = levelMusic;
    // asTo = lowHealthMusic;

    // On powerup start
    // isTransitioning = true;
    // asFrom = levelMusic;
    // asTo = powerupMusic;

    // on powerup end
    // isTransitioning = true;
    // asFrom = powerupMusic;
    // asTo = levelMusic;

    private void TransitionSongs () {

    }
}
