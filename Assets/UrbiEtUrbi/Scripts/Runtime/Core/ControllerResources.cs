using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerResources : MonoBehaviour
{
    public List<Resource> ResourceCollection;

    [SerializeField]
    List<ResourceAmount> resourceAmounts = new();


    private void Start()
    {
        //this should be loaded from the save file

        foreach (var r in ResourceCollection)
        {
            resourceAmounts.Add(new ResourceAmount (r.ID, 0 ));
        }
    }

    public Resource GetResourceByID(int id)
    {
        var idx = ResourceCollection.FindIndex(x => x.ID == id);
        if (idx != -1)
        {
            return ResourceCollection[idx];
        }
        else
        {
            return null;
        }
    }


    public void Change(ResourceAmount resourceAmount)
    {
        Change(resourceAmount.ID, resourceAmount.Amount);
    }

    public void Change(Resource resource, int amount)
    {

        Change(resource.ID, amount);
    }

    public void Change(int resourceID, int amount)
    {

        if (GetResourceAmount(resourceID, out var resAmount))
        {
            resAmount.Amount += amount;
        }
       
    }

    bool GetResourceAmount(int resourceID, out ResourceAmount amount)
    {
        var idx = resourceAmounts.FindIndex(x => x.ID == resourceID);
        if (idx != -1)
        {
            amount = resourceAmounts[idx];
            return true;
        }
        amount = default;
        return false;
    }

    public bool GetResourceData(int resourceID, out (Resource res, int amount) amount)
    {
        var idx = resourceAmounts.FindIndex(x => x.ID == resourceID);
        if (idx != -1)
        {
            amount = (GetResourceByID(resourceID), resourceAmounts[idx].Amount);
            return true;
        }
        amount = default;
        return false;
    }

    public int GetResourceAmount(int resourceID)
    {
        var idx = resourceAmounts.FindIndex(x => x.ID == resourceID);
        if (idx != -1)
        {
            return resourceAmounts[idx].Amount;
            
        }
        return 0;
    }

    public int GetResourceAmount(Resource resource)
    {
        var idx = resourceAmounts.FindIndex(x => x.ID == resource.ID);
        if (idx != -1)
        {
            return resourceAmounts[idx].Amount;

        }
        return 0;
    }



    public bool CanChange(ResourceAmount resourceAmount)
    {
        return CanChange(resourceAmount.ID, resourceAmount.Amount);
    }
    public bool CanChange(Resource resource, int amount) {
        return CanChange(resource.ID, amount);
    }
    public bool CanChange(int resourceID, int amount)
    {
        if (GetResourceAmount(resourceID, out var resAmount))
        {
            return resAmount.Amount + amount > 0;
        }
        return false;
    }
}
