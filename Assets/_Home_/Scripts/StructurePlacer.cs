using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructurePlacer : MonoBehaviour
{
    public Transform planet;
    public GameObject structureToPlace;
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

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    PlaceStructure(prefab);
        //}
        GetPlacePosition(out bool validPosition);

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
    public void PlaceStructure(GameObject prefabToPlace)
    {
        Vector3 forwardPosition = transform.position + transform.up * 2 + transform.forward * 3f;
        Vector3 downwardDirection = planet.position - forwardPosition;
        RaycastHit hit;
        Physics.Raycast(forwardPosition, downwardDirection, out hit, Mathf.Infinity);
        MeshCollider meshCollider = hit.collider as MeshCollider;
        Mesh mesh = meshCollider.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3 v0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 v1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 v2 = vertices[triangles[hit.triangleIndex * 3 + 2]];

        Vector3 structurePosition = (v0 + v1 + v2) / 3f;
        // Local to world
        Plane newPlane = new Plane(v0, v1, v2);
        Vector3 normal = meshCollider.transform.TransformDirection(newPlane.normal.normalized);
        structurePosition = meshCollider.transform.TransformPoint(structurePosition);
        Instantiate(prefabToPlace, structurePosition, Quaternion.LookRotation(transform.forward, normal));
    }
}