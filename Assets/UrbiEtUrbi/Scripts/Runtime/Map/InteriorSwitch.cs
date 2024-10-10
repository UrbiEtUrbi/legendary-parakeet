using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorSwitch : Interactible
{


    public override void OnInteract()
    {

        (TheGame.Instance.GameCycleManager.GetCurrentState as NightState).SwitchView();
        base.OnInteract();
    }
}
