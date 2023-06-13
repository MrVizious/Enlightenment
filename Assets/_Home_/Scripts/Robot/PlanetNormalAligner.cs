using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetNormalAligner : MonoBehaviour
{
    public Transform planet;

    private void FixedUpdate()
    {
        Vector3 up = transform.position - planet.position;
        transform.rotation = Quaternion.LookRotation(transform.forward, up);
    }
}
