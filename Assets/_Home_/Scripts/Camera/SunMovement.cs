using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMovement : MonoBehaviour
{
    public float rotationalSpeed = 5f;
    void Update()
    {
        transform.Rotate(transform.up, rotationalSpeed * Time.deltaTime);
    }
}
