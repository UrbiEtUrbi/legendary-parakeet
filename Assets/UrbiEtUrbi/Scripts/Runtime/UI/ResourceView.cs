using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PrimeTween;

public class ResourceView : PoolObject
{

    [SerializeField]
    Image Icon;

    [SerializeField]
    TMP_Text Label;

    public Resource Resource;

    int current;
    int currentAnimated;
    int target;

    Tween t;

    public void Init(Resource resource, int amount)
    {

        Icon.sprite = resource.Icon;
        Label.text = amount.ToString();
        Resource = resource;
        current = amount;
    }


    public void Init(int resourceID, int amount)
    {
        Init(TheGame.Instance.ControllerResources.GetResourceByID(resourceID), amount);
    }

    public void Set(int amount)
    {
        target = amount;

        if (t.isAlive)
        {
            t.Stop();
            current = currentAnimated;
        }

        t = Tween.Custom(0, 1f, duration: 0.5f, OnValueChanged).OnComplete(() => current = target);


        Label.text = amount.ToString();
    }

    void OnValueChanged(float value) {

        var amountNext = Mathf.RoundToInt((target - current)*value + current);
        currentAnimated = amountNext;
        Label.text = amountNext.ToString();
    }

   
}
