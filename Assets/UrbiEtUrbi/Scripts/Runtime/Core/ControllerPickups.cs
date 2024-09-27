using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllerPickups : MonoBehaviour
{

    public UnityEvent<Resource, int> OnPickupResource = new();
    public void Pickup(Pickup pickup)
    {


        switch (pickup)
        {
            case ResourcePickup resPickup:
                OnPickupResource.Invoke(resPickup.res, resPickup.amount);
                break;


        }

        TheGame.Instance.Save();
    }

}
