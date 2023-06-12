using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TypeReferences;
using Sirenix.OdinInspector;
using SerializableDictionaries;

public abstract class Resource : MonoBehaviour, ISpawnable
{

    [SerializeField]
    private TypeReferenceFloatDictionary _minimumDistanceToSpawnableTypes = new TypeReferenceFloatDictionary();
    public TypeReferenceFloatDictionary minimumDistanceToSpawnableTypes
    {
        get => _minimumDistanceToSpawnableTypes;
        set => _minimumDistanceToSpawnableTypes = value;
    }
    private float _defaultMinimumDistance = 5f;
    public float defaultMinimumDistance { get => _defaultMinimumDistance; set => _defaultMinimumDistance = value; }

}
