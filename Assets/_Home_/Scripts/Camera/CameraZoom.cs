using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CameraZoom : MonoBehaviour
{
    public float minFOV = 40f, maxFOV = 100f;
    public float dampeningSpeed = 2f;
    public float zoomStep = 15f;
    [ShowInInspector]
    public float desiredFOV
    {
        get => _desiredFOV;
        set
        {
            value = Mathf.Clamp(value, minFOV, maxFOV);
            _desiredFOV = value;
        }
    }
    private float _desiredFOV;
    private Camera _cam = null;
    public Camera cam
    {
        get
        {
            if (_cam == null)
            {
                _cam = GetComponent<Camera>();
            }
            if (_cam == null)
            {
                _cam = Camera.main;
            }
            return _cam;
        }
    }

    private void Start()
    {
        desiredFOV = cam.fieldOfView;
    }

    void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            desiredFOV -= zoomStep;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            desiredFOV += zoomStep;
        }
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, desiredFOV, dampeningSpeed * Time.deltaTime);
    }
}
