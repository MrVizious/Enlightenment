using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SerializableDictionaries;
using UtilityMethods;
using TypeReferences;
using GameEvents;


public class Structure : MonoBehaviour
{
    public GameEvent onBuilt, onPlanned;
    public ResourceSOIntDictionary resourcesNeeded;
    public UnityEvent onResourcesNeededChanged = new UnityEvent();
    public Material placingInvalidMaterial, placingValidMaterial, plannedMaterial, buildingMaterial, builtMaterial;
    [System.Serializable]
    public enum StructureState
    {
        Placing_invalid,
        Placing_valid,
        Planned,
        Building,
        Built
    }

    public StructureState state
    {
        get => _state;
        set
        {
            // Change to invalid
            if (value == StructureState.Placing_invalid)
            {
                switch (state)
                {
                    case StructureState.Placing_invalid:
                    case StructureState.Placing_valid:
                        SetMaterial(placingInvalidMaterial);
                        _state = value;
                        break;
                    default: break;
                }
            }
            // Change to valid
            else if (value == StructureState.Placing_valid)
            {
                switch (state)
                {
                    case StructureState.Placing_valid:
                    case StructureState.Placing_invalid:
                        SetMaterial(placingValidMaterial);
                        _state = value;
                        break;
                    default: break;
                }
            }
            // Change to planned
            else if (value == StructureState.Planned)
            {
                switch (state)
                {
                    case StructureState.Planned:
                    case StructureState.Placing_valid:
                        SetMaterial(plannedMaterial);
                        onPlanned.Raise();
                        _state = value;
                        break;
                    default: break;
                }
            }
            // Change to building
            else if (value == StructureState.Building)
            {
                switch (state)
                {
                    case StructureState.Building:
                    case StructureState.Planned:
                        SetMaterial(buildingMaterial);
                        _state = value;
                        break;
                    default: break;
                }
            }
            // Change to built
            else if (value == StructureState.Built)
            {
                switch (state)
                {
                    case StructureState.Built:
                    case StructureState.Building:
                        SetMaterial(builtMaterial);
                        _state = value;
                        onBuilt.Raise();
                        break;
                    default: break;
                }
            }
        }
    }

    private StructureState _state = StructureState.Placing_invalid;
    private List<Resource> ownedResources = new List<Resource>();
    private void SetMaterial(Material newMaterial)
    {
        GetComponentInChildren<MeshRenderer>().material = newMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (state != StructureState.Building && state != StructureState.Planned) return;
        Resource resource = other.GetComponent<Resource>();
        if (resource == null) return;

        ResourceSO resourceData = resource.data;
        Debug.Log(resourceData);
        if (!resourcesNeeded.ContainsKey(resourceData)) return;
        if (resourcesNeeded[resourceData] <= 0) return;

        state = StructureState.Building;
        ownedResources.Add(resource);
        PlaceResourceOnFloor(resource);
        resourcesNeeded[resourceData]--;
        onResourcesNeededChanged.Invoke();
        if (IsComplete())
        {
            foreach (Resource resourceToDestroy in ownedResources)
            {
                Destroy(resourceToDestroy.gameObject);
            }
            state = StructureState.Built;
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