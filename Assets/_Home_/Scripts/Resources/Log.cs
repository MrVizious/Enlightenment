using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Log : MonoBehaviour, ISpawnable
{
    public GameObject logPrefab;
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

    public float minimumDistanceToSameSpawnable { get; set; }

    [Button]
    public Vector3 GenerateVector()
    {
        Vector3 newVector = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180)).normalized;
        Debug.DrawRay(planet.transform.position, newVector * 60f, Color.blue, 5f);
        return newVector;
    }

    public Vector3 CalculateSpawnPoint()
    {
        Vector3 position = Vector3.zero;
        Vector3 spawnDirection = GenerateVector();

        // Raycast towarsd vector to find collision point
        RaycastHit[] hits;
        Vector3 initialPosition = planet.transform.position + (spawnDirection * 60f);
        Vector3 finalPosition = planet.transform.position;
        Vector3 rayDirection = finalPosition - initialPosition;
        hits = Physics.RaycastAll(initialPosition, rayDirection, Mathf.Infinity);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider is not MeshCollider) continue;
            if (((MeshCollider)hit.collider).sharedMesh != planetMesh) continue;

            Debug.Log("Hit planet!");
            position = hit.point;
            Vector3 normal = hit.normal;
            Vector3 forward = Vector3.Cross(Random.insideUnitSphere, normal).normalized;
            Quaternion newRotation = Quaternion.LookRotation(forward, hit.normal);
            Instantiate(logPrefab, position, newRotation);
            return position;
        }

        return position;
    }

    [Button]
    public void Spawn()
    {
        //Instantiate(logPrefab, CalculateSpawnPoint(), Quaternion.identity);
        CalculateSpawnPoint();
    }

}
