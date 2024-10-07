using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AmmoDeposit : Interactible
{
    [SerializeField]
    Resource AmmoType;


    InteriorController InteriorController;

    public void Init(InteriorController interiorController)
    {
        InteriorController = interiorController;
    }

    protected override bool CanInteract()
    {
        return InteriorController.CarryingType == AmmoType;
      
    }

    public override void OnInteract()
    {
        InteriorController.DepositAmmo(AmmoType, this);
        base.OnInteract();
    }
}
