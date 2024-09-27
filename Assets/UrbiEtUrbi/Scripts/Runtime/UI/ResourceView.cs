using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceView : PoolObject
{

    [SerializeField]
    Image Icon;

    [SerializeField]
    TMP_Text Label;

    public Resource Resource;

    public void Init(Resource resource, int amount)
    {

        Icon.sprite = resource.Icon;
        Label.text = amount.ToString();
        Resource = resource;
    }


    public void Init(int resourceID, int amount)
    {
        Init(TheGame.Instance.ControllerResources.GetResourceByID(resourceID), amount);
    }

    public void Set(int amount)
    {
        Label.text = amount.ToString();
    }

   
}
