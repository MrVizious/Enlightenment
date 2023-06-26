using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureResourcesNeededUI : MonoBehaviour
{
    public ResourceNeededUI bubblePrefab;
    public Structure structure;
    private Dictionary<ResourceSO, ResourceNeededUI> bubblesDictionary = new Dictionary<ResourceSO, ResourceNeededUI>();

    private void Start()
    {
        CreateBubbles();
        structure.onResourcesNeededChanged.AddListener(UpdateNumbers);
    }
    private void CreateBubbles()
    {
        foreach (ResourceSO resource in structure.resourcesNeeded.Keys)
        {
            ResourceNeededUI neededUI = Instantiate(bubblePrefab, transform.position, transform.rotation, transform);
            neededUI.SetData(resource, structure.resourcesNeeded[resource]);
            bubblesDictionary.Add(resource, neededUI);
        }
    }

    private void UpdateNumbers()
    {
        structure.onResourcesNeededChanged.RemoveListener(UpdateNumbers);
        foreach (ResourceSO resource in structure.resourcesNeeded.Keys)
        {
            ResourceNeededUI ui = bubblesDictionary[resource];
            if (ui == null)
            {
                Debug.Log("Empty for " + resource + "!");
                continue;
            }
            ui.SetAmountNeeded(structure.resourcesNeeded[resource]);
        }
        structure.onResourcesNeededChanged.AddListener(UpdateNumbers);
    }
}