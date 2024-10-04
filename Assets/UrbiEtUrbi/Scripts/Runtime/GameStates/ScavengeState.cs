using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using PrimeTween;

public class ScavengeState : GameState
{

    [SerializeField]
    PixelPerfectFollowCamera cam;

    [SerializeField]
    ScavangeResourceView ScavangedResourceView;


    List<ResourceAmount> CollectedResources = new();


    [SerializeField]
    Transform MapParent;

    [SerializeField]
    CinemachineVirtualCamera vCam;


    [SerializeField]
    List<Map> Prefabs;

    Map currentMap;

    [SerializeField]
    PopupScavengeEndDay Popup;

    public override void Init()
    {
        base.Init();

       // cam.target = Player.transform;
        vCam.Follow = Player.transform;
        TheGame.Instance.ControllerPickups.OnPickupResource.AddListener(CollectResource);

       
        currentMap = Instantiate<Map>(Prefabs[0]);
       vCam.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = currentMap.Confiner;

    }
    public override void Cleanup()
    {    
        base.Cleanup();
        TheGame.Instance.ControllerPickups.OnPickupResource.RemoveListener(CollectResource);
        currentMap.Clear();
        Destroy(currentMap.gameObject);
    }

    public void HidePopup()
    {
        Popup.Hide();
        Tween.Delay(0.6f, () => TheGame.Instance.GameCycleManager.CheatNextState());
    }


    public override void OnEndStage()
    {
        foreach (var ra in CollectedResources)
        {
            TheGame.Instance.ControllerResources.Change(ra);
        }
        Popup.Init(CollectedResources);
        base.OnEndStage();
    }



    public void CollectResource(Resource resource, int amount, Pickup pickup)
    {
        var idx = CollectedResources.FindIndex(x => x.ID == resource.ID);
        if (idx != -1)
        {
            CollectedResources[idx].Amount += amount;
        }
        else
        {
            CollectedResources.Add(new ResourceAmount
            (
                resource.ID,
                amount
            ));
            idx = CollectedResources.Count - 1;
        }

        ScavangedResourceView.UpdateResourceCollected(CollectedResources[idx]);
        currentMap.PickedUp(pickup);
        if (currentMap.HasPickedUpAll)
        {
            
            TheGame.Instance.GameCycleManager.EndStage();
        }
    }
}
