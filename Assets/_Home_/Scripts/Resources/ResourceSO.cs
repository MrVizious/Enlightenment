using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TypeReferences;
using SerializableDictionaries;
using Sirenix.OdinInspector;

[System.Serializable]
[CreateAssetMenu(fileName = "ResourceSO", menuName = "Enlightenment/ResourceSO", order = 0)]
public class ResourceSO : ScriptableObject
{
    [ShowInInspector]
    public static float defaultMinimumDistanceToResource = 10f;
    public ResourceSOFloatDictionary minimumDistanceToResource;
    public Resource prefab;
    public float weight;
}

[System.Serializable]
public class ResourceSOFloatDictionary : SerializableDictionary<ResourceSO, float> { }