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
    public UnityEvent onDropped, onBeingCarried;
    [SerializeField]
    private bool _isBeingCarried = false;
    public bool isBeingCarried
    {
        get => _isBeingCarried;
        set
        {
            if (value) onBeingCarried.Invoke();
            else onDropped.Invoke();
            _isBeingCarried = value;
        }
    }
}