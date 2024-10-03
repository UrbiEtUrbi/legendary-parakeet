using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePickupInteractible : Interactible
{

    public override void OnInteract()
    {

        var rp = GetComponent<ResourcePickup>();
        rp.PickupResource();
        if (DestroyOnInteract)
        {
            PoolManager.Despawn(rp);
        }
    }
}
