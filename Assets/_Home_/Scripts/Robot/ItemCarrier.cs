using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ItemCarrier : MonoBehaviour
{
    private AIPathAlignedToSurface _aiMover;
    private AIPathAlignedToSurface aiMover
    {
        get
        {
            if (_aiMover == null) _aiMover = GetComponent<AIPathAlignedToSurface>();
            return _aiMover;
        }
    }
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
            if (pickedUpResource == null) PickUpResource();
            else DropResource();
        }
    }

    public void PickUpResource()
    {
        if (pickedUpResource != null) return;

        if (resourcesInRange.Count <= 0) return;
        pickedUpResource = resourcesInRange[0];
        pickedUpResource.transform.position = transform.position + transform.up * 2.5f;
        pickedUpResource.transform.SetParent(transform);
        aiMover.maxSpeed *= pickedUpResource.data.speedModifier;
        pickedUpResource.onDropped.AddListener(ForgetResource);
    }

    public void DropResource()
    {
        if (pickedUpResource == null) return;
        pickedUpResource.transform.SetParent(null);
        pickedUpResource.transform.position = transform.position;
        pickedUpResource.transform.rotation = transform.rotation;
        ForgetResource();
    }

    private void ForgetResource()
    {
        if (pickedUpResource == null) return;
        pickedUpResource.onDropped.RemoveListener(ForgetResource);
        aiMover.maxSpeed /= pickedUpResource.data.speedModifier;
        pickedUpResource = null;
    }
}
