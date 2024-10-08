using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPopupInteractible : Interactible
{
    [SerializeField]
    PopupBase PopupBase;

    InteriorController InteriorController;

    public void Init(InteriorController interiorController)
    {
        InteriorController = interiorController;
    }

    public override void OnInteract()
    {
        InteriorController.OnPopupOpened();
        PopupBase.Show();
        base.OnInteract();
    }
}
