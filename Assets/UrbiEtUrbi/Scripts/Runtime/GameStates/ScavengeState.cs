using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScavengeState : GameState
{

    [SerializeField]
    CinemachineVirtualCamera vCam;

    public override void Init()
    {
        base.Init();
        vCam.Follow = Player.transform;
    }

    public override void Cleanup()
    {

      
        base.Cleanup();
    }
}
