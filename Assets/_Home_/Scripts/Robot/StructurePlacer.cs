using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructurePlacer : MonoBehaviour
{
    public Transform planet;
    public GameObject prefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlaceStructure(prefab);
        }
    }

    public void PlaceStructure(GameObject prefabToPlace)
    {
        Vector3 forwardPosition = transform.position + transform.up * 2 + transform.forward * 3f;
        Vector3 downwardDirection = planet.position - forwardPosition;
        RaycastHit hit;
        Physics.Raycast(forwardPosition, downwardDirection, out hit, Mathf.Infinity);
        MeshCollider meshCollider = hit.collider as MeshCollider;
        Debug.Log(meshCollider);
        Debug.Log(hit.triangleIndex);
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