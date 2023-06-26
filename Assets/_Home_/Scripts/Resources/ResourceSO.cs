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
    public Sprite icon;
    public static float defaultMinimumDistanceToResource = 10f;
    public ResourceSOIntDictionary minimumDistanceToResource;
    public Resource prefab;
    [Range(0f, 1f)]
    public float speedModifier;
}

[System.Serializable]
public class ResourceSOIntDictionary : SerializableDictionary<ResourceSO, int> { }