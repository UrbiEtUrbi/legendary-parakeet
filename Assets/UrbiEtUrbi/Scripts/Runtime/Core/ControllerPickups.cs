using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPickups : MonoBehaviour
{
    public void Pickup(Pickup pickup)
    {


        switch (pickup.PickupType)
        {
            case PickupType.MaxHealth:
                break;


        }

        TheGame.Instance.Save();

    }

}
