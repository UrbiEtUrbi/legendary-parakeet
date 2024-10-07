using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItemInteractible : Interactible
{

    [SerializeField]
    Resource Resource;

    public int Amount;




    InteriorController InteriorController;

    public void Init(InteriorController interiorController)
    {
        InteriorController = interiorController;
    }

    public override void OnInteract()
    {

        InteriorController.PickupResource(Resource, Amount);
        base.OnInteract();
    }
}
