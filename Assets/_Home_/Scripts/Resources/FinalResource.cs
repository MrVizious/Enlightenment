using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalResource : Resource
{
    private void OnTriggerExit(Collider other)
    {
        if (!other.tag.ToLower().Equals("sun")) return;
        if (!isBeingCarried) return;

        Debug.Log("Explode!");

    }
}
