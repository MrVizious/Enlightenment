using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TypeReferences;

public class ResourceSpawner : MonoBehaviour
{
    public ResourceSO logSO, rockSO;
    public GameObject planet;
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

    private List<Resource> spawnedResources = new List<Resource>();

    private void Start()
    {
        SpawnRock();
        SpawnRock();
        SpawnRock();
        SpawnRock();
        SpawnRock();
        SpawnRock();
        SpawnLog();
        SpawnLog();
        SpawnLog();
        SpawnLog();
        SpawnLog();
    }


    [Button]
    public void SpawnRock()
    {
        Spawn(rockSO.prefab);
    }
    [Button]
    public void SpawnLog()
    {
        Spawn(logSO.prefab);
    }
    public Resource Spawn(Resource resourceToSpawn)
    {
        (Vector3, Quaternion) positionAndRotation = CalculateValidSpawnPoint(resourceToSpawn);
        Resource newResource = (Resource)Instantiate(resourceToSpawn, positionAndRotation.Item1, positionAndRotation.Item2);
        newResource.transform.SetParent(transform);
        spawnedResources.Add(newResource);
        return newResource;
    }
    public Vector3 GenerateVector()
    {
        Vector3 newVector = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180)).normalized;
        //Debug.DrawRay(planet.transform.position, newVector * 60f, Color.blue, 5f);
        return newVector;
    }

    public (Vector3, Quaternion) CalculateSpawnPoint()
    {
        Vector3 position = Vector3.zero;
        Vector3 spawnDirection = GenerateVector();

        // Raycast towarsd vector to find collision point
        RaycastHit[] hits;
        Vector3 initialRayPosition = planet.transform.position + (spawnDirection * planet.transform.lossyScale.magnitude);
        Vector3 finalRayPosition = planet.transform.position;
        Vector3 rayDirection = finalRayPosition - initialRayPosition;
        hits = Physics.RaycastAll(initialRayPosition, rayDirection, Mathf.Infinity);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider is not MeshCollider) continue;
            if (((MeshCollider)hit.collider).sharedMesh != planetMesh) continue;

            position = hit.point;
            Vector3 normal = hit.normal;
            Vector3 forward = Vector3.Cross(Random.insideUnitSphere, normal).normalized;
            forward = Vector3.ProjectOnPlane(forward, normal);
            Quaternion newRotation = Quaternion.LookRotation(forward, position - hit.collider.transform.position);
            //Instantiate(logPrefab, position, newRotation);

            return (position, newRotation);
        }

        return (position, Quaternion.identity);
    }

    public (Vector3, Quaternion) CalculateValidSpawnPoint(Resource resourceToSpawn)
    {
        (Vector3, Quaternion) positionAndRotation = CalculateSpawnPoint();
        if (spawnedResources.Count <= 0)
        {
            return positionAndRotation;
        }
        foreach (Resource spawnedResource in spawnedResources)
        {
            float distance = Vector3.Distance(spawnedResource.transform.position, positionAndRotation.Item1);
            float minimumDistance = 0f;
            // There is not an specified distance for the spawned resource
            if (!resourceToSpawn.data.minimumDistanceToResource.ContainsKey(spawnedResource.data))
            {
                minimumDistance = ResourceSO.defaultMinimumDistanceToResource;
            }
            else
            {
                minimumDistance = resourceToSpawn.data.minimumDistanceToResource[spawnedResource.data];
            }
            if (distance < minimumDistance) return CalculateValidSpawnPoint(resourceToSpawn);
        }
        return positionAndRotation;
    }
}
