using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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


    public void CollectResource(Resource resource, int amount)
    {
        var idx = CollectedResources.FindIndex(x => x.ID == resource.ID);
        if (idx != -1)
        {
            CollectedResources[idx].Amount += amount;
        }
        else
        {
            CollectedResources.Add(new ResourceAmount
            {
                ID = resource.ID,
                Amount = amount
            });
            idx = CollectedResources.Count - 1;
        }

        ScavangedResourceView.UpdateResourceCollected(CollectedResources[idx]);

    }
}
