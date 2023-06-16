using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TypeReferences;
using Sirenix.OdinInspector;
using SerializableDictionaries;

public class Resource : MonoBehaviour
{
    public ResourceSO data;
    public UnityEvent onDropped;
    public bool isBeingCarried = false;
}