using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TypeReferences;
using SerializableDictionaries;

public interface ISpawnable
{
    [ShowInInspector]
    TypeReferenceFloatDictionary minimumDistanceToSpawnableTypes
    {
        get; set;
    }
    float defaultMinimumDistance { get; set; }
}
