using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float zoomTarget;
    public float inTime;
    public float outTime;
    private float ogSize;
    private Camera cam;
    private float velocity = 0.0f;
    public int moveState;

    private void OnEnable () {
        TypingController.SpellFired += StartZoom;
    }

    private void OnDisable () {
        TypingController.SpellFired -= StartZoom;
    }

    // Start is called before the first frame update
    void Start()
    {
      cam = Camera.main;
      moveState = 0;
      ogSize = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
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
