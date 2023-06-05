using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivotMovement : MonoBehaviour
{
    public enum CameraControlType
    {
        Simple,
        Advanced
    }
    public CameraControlType cameraControlType = CameraControlType.Advanced;
    private void Update()
    {
        float verticalMovement = Input.GetAxis("Vertical");
        transform.Rotate(Vector3.right, 30 * verticalMovement * Time.deltaTime);
        float rotationalMovement = 0f;
        if (cameraControlType == CameraControlType.Simple)
        {
            rotationalMovement = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.forward, -60 * rotationalMovement * Time.deltaTime);
        }
        else if (cameraControlType == CameraControlType.Advanced)
        {
            float horizontalMovement = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up, -30 * horizontalMovement * Time.deltaTime);

            rotationalMovement = Input.GetAxis("Rotational");
            transform.Rotate(Vector3.forward, -60 * rotationalMovement * Time.deltaTime);
        }

    }
}
