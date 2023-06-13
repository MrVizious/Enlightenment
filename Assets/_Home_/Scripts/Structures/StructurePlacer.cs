using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class StructurePlacer : MonoBehaviour
{
    public Transform planet;
    public Structure structureToPlace;
    private Mesh _planetMesh;
    private Mesh planetMesh
    {
        get
        {
            if (_planetMesh == null)
            {
                _planetMesh = planet.GetComponentInChildren<MeshCollider>().sharedMesh;
            }
            return _planetMesh;
        }
    }

    [Button]
    public void PlaceStructure(Structure structurePrefab)
    {
        structureToPlace = Instantiate(structurePrefab).GetComponentInChildren<Structure>();
    }

    private void Update()
    {
        if (structureToPlace == null) return;
        GetPlacePosition(out bool validPosition);
        structureToPlace.state =
            IsValidPosition(structureToPlace.transform.position) ?
                Structure.StructureState.placing_valid
                : Structure.StructureState.placing_invalid;
        if (Input.GetMouseButtonDown(0))
        {
            if (structureToPlace.state == Structure.StructureState.placing_valid)
            {
                structureToPlace.state = Structure.StructureState.planned;
                structureToPlace = null;
            }
        }
    }

    private void GetPlacePosition(out bool validPosition)
    {
        validPosition = false;
        // Raycast towarsd vector to find collision point
        RaycastHit[] hits;
        Vector3 initialRayPosition = Camera.main.transform.position;
        Vector3 finalRayPosition = planet.transform.position;
        Vector3 rayDirection = finalRayPosition - initialRayPosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hits = Physics.RaycastAll(initialRayPosition, ray.direction, Vector3.Distance(Camera.main.transform.position, planet.transform.position));

        if (hits.Length <= 0) return;
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider is not MeshCollider) continue;
            if (((MeshCollider)hit.collider).sharedMesh != planetMesh) continue;
            Vector3 normal = hit.normal;
            Vector3 forward = Camera.main.transform.up.normalized;
            Quaternion newRotation = Quaternion.LookRotation(forward, normal);
            structureToPlace.transform.position = hit.point;
            structureToPlace.transform.rotation = newRotation;
        }

        //Instantiate(logPrefab, position, newRotation);

        //return (position, newRotation);
    }

    private bool IsValidPosition(Vector3 position, float radius = 4f)
    {
        bool isValidPosition = true;
        Collider[] hitColliders = Physics.OverlapSphere(position, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform.parent == structureToPlace) continue;
            Resource resource = hitCollider.GetComponentInChildren<Resource>();
            Robot robot = hitCollider.GetComponentInChildren<Robot>();
            // TODO: Check structure collisions with other than this
            if (resource != null || robot != null)
            {
                isValidPosition = false;
                break;
            }
        }
        DebugExtension.DebugWireSphere(position, isValidPosition ? Color.green : Color.red, radius);
        return isValidPosition;
    }
}