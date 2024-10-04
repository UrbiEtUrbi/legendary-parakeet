using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllerPickups : MonoBehaviour
{

    public UnityEvent<Resource, int, Pickup> OnPickupResource = new();
    public void Pickup(Pickup pickup)
    {


        switch (pickup)
        {
            case ResourcePickup resPickup:
                var drops = resPickup.cache.Open();
                foreach (var d in drops)
                {
                    var res = TheGame.Instance.ControllerResources.GetResourceByID(d.ID);
                    OnPickupResource.Invoke(res, d.Amount, pickup);
                }
                break;


        }

        TheGame.Instance.Save();
    }

}
