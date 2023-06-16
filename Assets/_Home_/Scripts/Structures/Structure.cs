using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializableDictionaries;
using UtilityMethods;
using TypeReferences;


public class Structure : MonoBehaviour
{
    public ResourceSOFloatDictionary resourcesNeeded;
    public Material placingInvalidMaterial, placingValidMaterial, plannedMaterial, buildingMaterial, builtMaterial;
    [System.Serializable]
    public enum StructureState
    {
        placing_invalid,
        placing_valid,
        planned,
        building,
        built
    }

    public StructureState state
    {
        get => _state;
        set
        {
            // Change to invalid
            if (value == StructureState.placing_invalid)
            {
                switch (state)
                {
                    case StructureState.placing_invalid:
                    case StructureState.placing_valid:
                        SetMaterial(placingInvalidMaterial);
                        _state = value;
                        break;
                    default: break;
                }
            }
            // Change to valid
            else if (value == StructureState.placing_valid)
            {
                switch (state)
                {
                    case StructureState.placing_valid:
                    case StructureState.placing_invalid:
                        SetMaterial(placingValidMaterial);
                        _state = value;
                        break;
                    default: break;
                }
            }
            // Change to planned
            else if (value == StructureState.planned)
            {
                switch (state)
                {
                    case StructureState.planned:
                    case StructureState.placing_valid:
                        SetMaterial(plannedMaterial);
                        _state = value;
                        break;
                    default: break;
                }
            }
            // Change to building
            else if (value == StructureState.building)
            {
                switch (state)
                {
                    case StructureState.building:
                    case StructureState.planned:
                        SetMaterial(buildingMaterial);
                        _state = value;
                        break;
                    default: break;
                }
            }
            // Change to built
            else if (value == StructureState.built)
            {
                switch (state)
                {
                    case StructureState.built:
                    case StructureState.building:
                        SetMaterial(builtMaterial);
                        _state = value;
                        break;
                    default: break;
                }
            }
        }
    }

    private StructureState _state = StructureState.placing_invalid;
    private List<Resource> ownedResources = new List<Resource>();
    private void SetMaterial(Material newMaterial)
    {
        GetComponentInChildren<MeshRenderer>().material = newMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state != StructureState.building && state != StructureState.planned) return;
        Resource resource = other.GetComponent<Resource>();
        if (resource == null) return;

        ResourceSO resourceData = resource.data;
        Debug.Log(resourceData);
        if (!resourcesNeeded.ContainsKey(resourceData)) return;
        if (resourcesNeeded[resourceData] <= 0) return;

        state = StructureState.building;
        ownedResources.Add(resource);
        PlaceResourceOnFloor(resource);
        resourcesNeeded[resourceData]--;
        if (IsComplete())
        {
            foreach (Resource resourceToDestroy in ownedResources)
            {
                Destroy(resourceToDestroy.gameObject);
            }
            state = StructureState.built;
        }
    }

    private bool IsComplete()
    {
        foreach (ResourceSO neededResourceData in resourcesNeeded.Keys)
        {
            if (resourcesNeeded[neededResourceData] > 0) return false;
        }
        return true;
    }
    private void PlaceResourceOnFloor(Resource resource)
    {
        // Disable collisions
        resource.GetComponent<Collider>().enabled = false;
        Vector3 normal = transform.up;

        // Get new position in radius
        Vector3 resourceNewPosition = Math.RandomPointOnPlane(transform.position, normal, 0.4f);

        // Get new rotation for resource
        Vector3 forward = Vector3.Cross(Random.insideUnitSphere, normal).normalized;
        Quaternion newRotation = Quaternion.LookRotation(forward, normal);

        // Set position and rotation
        resource.transform.position = resourceNewPosition;
        resource.transform.rotation = newRotation;
        resource.transform.SetParent(null);


        resource.onDropped.Invoke();
    }

}