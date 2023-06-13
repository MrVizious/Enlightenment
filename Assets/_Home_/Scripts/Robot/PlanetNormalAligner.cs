using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetNormalAligner : MonoBehaviour
{
    public Transform planet;

    private void FixedUpdate()
    {
        transform.up = transform.position - planet.position;
    }
}
