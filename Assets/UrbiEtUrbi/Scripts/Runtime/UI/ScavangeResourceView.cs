using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;

public class ScavangeResourceView : MonoBehaviour
{

    List<ResourceView> Views = new();

    [SerializeField]
    float Width, Height;


    public void UpdateResourceCollected(ResourceAmount resourceAmount)
    {
        var res = TheGame.Instance.ControllerResources.GetResourceByID(resourceAmount.ID);
        UpdateResourceCollected(res, resourceAmount.Amount);
    }

    public void UpdateResourceCollected(Resource resource, int amount)
    {

        var idx = Views.FindIndex(x => x.Resource == resource);
        if (idx != -1)
        {
            Views[idx].Set(amount);
        }
        else
        {
            var v = PoolManager.Spawn<ResourceView>(transform);
            var rt = v.transform as RectTransform;
            var nextIndex = Views.Count;
            rt.anchoredPosition = new Vector2(-Width, nextIndex * Height);
            Tween.UIAnchoredPosition(rt, new Vector2(10, nextIndex * Height), duration: 0.5f);
            v.Init(resource, 0);
            v.Set(amount);
            Views.Add(v);
        }
    }

    public void Clear()
    {
        foreach (var v in Views)
        {
            PoolManager.Despawn(v);
        }
        Views.Clear();
    }
}
