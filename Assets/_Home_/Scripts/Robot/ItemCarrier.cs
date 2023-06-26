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
    private Robot _robot;
    private Robot robot
    {
        get
        {
            if (_robot == null) _robot = GetComponent<Robot>();
            return _robot;
        }
    }
    private Animator _animator;
    private Animator animator
    {
        get
        {
            if (_animator == null) _animator = GetComponent<Animator>();
            return _animator;
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
        Debug.Log(other);
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
        pickedUpResource.isBeingCarried = true;
        aiMover.maxSpeed *= pickedUpResource.data.speedModifier;
        animator.SetFloat("SpeedMultiplier", pickedUpResource.data.speedModifier);
        robot.drainingSpeed += (1f - pickedUpResource.data.speedModifier) * 0.1f;
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
        pickedUpResource.isBeingCarried = false;
        resourcesInRange.Remove(pickedUpResource);
        aiMover.maxSpeed /= pickedUpResource.data.speedModifier;
        animator.SetFloat("SpeedMultiplier", 1f);
        robot.drainingSpeed -= (1f - pickedUpResource.data.speedModifier) * 0.1f;
        pickedUpResource = null;
    }
}
