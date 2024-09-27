using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResourcePickup : Pickup
{
    [HideInInspector]
    public Resource res;

    [HideInInspector]
    public int amount;

    [SerializeField]
    SpriteRenderer SpriteRenderer;

    public void Init(Resource res, int amount)
    {
       this. amount = amount;
       this.res = res;
       SpriteRenderer.sprite = res.Icon;
        base.Init();
    }

    public void Init(int resID, int amount)
    {
        Init(TheGame.Instance.ControllerResources.GetResourceByID(resID), amount);
    }
}
