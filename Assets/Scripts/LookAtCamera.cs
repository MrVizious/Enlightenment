using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField]
    private Camera _cam = null;
    public Camera cam
    {
        get
        {
            if (_cam == null)
            {
                _cam = Camera.main;
            }
            return _cam;
        }
    }
    private RectTransform _rectTransform;
    private RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //rectTransform.LookAt(cam.transform);
        rectTransform.rotation = Quaternion.LookRotation(cam.transform.forward, cam.transform.up);
        //rectTransform.up = cam.transform.up;
    }
}
