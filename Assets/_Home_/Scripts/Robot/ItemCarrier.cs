using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCarrier : MonoBehaviour
{
    [SerializeField]
    private Resource pickedUpResource = null;
    [SerializeField] List<Resource> resourcesInRange = new List<Resource>();
    private void OnTriggerEnter(Collider other)
    {
        Resource resource = other.GetComponent<Resource>();
        if (resource == null) return;
        if (resourcesInRange.Contains(resource)) return;
        resourcesInRange.Add(resource);
    }

    private void OnTriggerExit(Collider other)
    {
        Resource resource = other.GetComponent<Resource>();
        if (resource == null) return;
        if (resourcesInRange.Contains(resource)) resourcesInRange.Remove(resource);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (pickedUpResource == null)
            {
                if (resourcesInRange.Count <= 0) return;
                pickedUpResource = resourcesInRange[0];
                pickedUpResource.transform.position = transform.position + transform.up * 2.5f;
                pickedUpResource.transform.SetParent(transform);
            }
            else
            {
                pickedUpResource.transform.SetParent(null);
                pickedUpResource.transform.position = transform.position;
                pickedUpResource.transform.rotation = transform.rotation;
                pickedUpResource = null;
            }

        }
    }
}
