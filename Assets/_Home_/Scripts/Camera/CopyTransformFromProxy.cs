using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransformFromProxy : MonoBehaviour
{
    public Transform proxy;
    void Update()
    {
        transform.position = proxy.position;
        transform.rotation = proxy.rotation;
        transform.localScale = proxy.localScale;
    }
}
