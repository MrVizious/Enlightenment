using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPivotMovement : MonoBehaviour
{
    private void Update()
    {
        float verticalMovement = Input.GetAxis("Vertical");
        transform.Rotate(Vector3.right, 30 * verticalMovement * Time.deltaTime);

        float horizontalMovement = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, -30 * horizontalMovement * Time.deltaTime);

        float rotationalMovement = Input.GetAxis("Rotational");
        transform.Rotate(Vector3.forward, -60 * rotationalMovement * Time.deltaTime);
    }
}
