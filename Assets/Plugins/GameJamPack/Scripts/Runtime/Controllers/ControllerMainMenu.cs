using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMainMenu : ControllerLocal
{


    [SerializeField, SceneDetails]
    SerializedScene Scene;


    public override void Init()
    {
        base.Init();
        OnPlay();
        
    }

    public void OnPlay()
    {
        ControllerGameFlow.Instance.LoadNewScene(Scene.BuildIndex);
    }



    

 
}
