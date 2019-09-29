using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomTarget;
    public float inTime;
    public float outTime;
    private float ogSize;
    Vector3 originalPos;
    private Camera cam;
    private float velocity = 0.0f;
    public int moveState;

    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    private void OnEnable () {
        TypingController.SpellFired += StartZoom;
        WizardPlayer.HealthLost += StartScreenShake;
    }

    private void OnDisable () {
        TypingController.SpellFired -= StartZoom;
        WizardPlayer.HealthLost -= StartScreenShake;
    }

    // Start is called before the first frame update
    void Start()
    {
      cam = Camera.main;
      moveState = 0;
      ogSize = cam.orthographicSize;
      originalPos = gameObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
        ScreenShake ();
    }

    public void StartScreenShake (bool start)
    {
        if (!start)
            return;
        shakeDuration = 0.3f;
    }

    public void ScreenShake ()
    {
        if (shakeDuration > 0)
        {
            gameObject.transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            gameObject.transform.localPosition = originalPos;
        }
    }

    public void StartZoom()
    {
        moveState = 1;
    }

    private void Zoom()
    {
        if (moveState == 1)
        {
            if (cam.orthographicSize - zoomTarget > 0.01f)
            {
                float newSize = Mathf.SmoothDamp(cam.orthographicSize, zoomTarget, ref velocity, inTime);
                cam.orthographicSize = newSize;
            }
            else
                moveState = -1;
        }
        else if (moveState == -1)
        {
            if (cam.orthographicSize - ogSize < -0.01f)
            {
                float newSize = Mathf.SmoothDamp(cam.orthographicSize, ogSize + 0.1f, ref velocity, outTime);
                cam.orthographicSize = newSize;
            }
            else
            {
                moveState = 0;
                cam.orthographicSize = ogSize;
            }
        }
    }
}
