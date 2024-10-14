using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesDisplay : MonoBehaviour
{
    [SerializeField]
    RectTransform container;

    List<ResourceView> Views = new List<ResourceView>();



    public void OnEnable()
    {

        
        foreach (var r in TheGame.Instance.ControllerResources.ResourceCollection)
        {

            var amount = TheGame.Instance.ControllerResources.GetResourceAmount(r);
            if (amount == 0)
            {
                continue;
            }
            var view = PoolManager.Spawn<ResourceView>("ResourceViewEndDay", container);
            view.Init(r.ID, amount);
            Views.Add(view);
        }

    }

    private void OnDisable()
    {
        for (int i = Views.Count - 1; i >= 0; i--)
        {
            PoolManager.Despawn(Views[i]);
        }
        Views.Clear();
    }
}
