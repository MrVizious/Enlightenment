using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
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

    private StructureState _state = StructureState.placing_invalid;
    [SerializeField]
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
                        Debug.Log("Changing to invalid");
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
                        Debug.Log("Changing to valid");
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
                        Debug.Log("Changing to planned");
                        SetMaterial(plannedMaterial);
                        _state = value;
                        break;
                    default: break;
                }
            }
        }
    }

    private void SetMaterial(Material newMaterial)
    {
        GetComponentInChildren<MeshRenderer>().material = newMaterial;
    }

}
