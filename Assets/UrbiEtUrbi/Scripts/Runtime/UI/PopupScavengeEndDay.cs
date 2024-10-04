using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupScavengeEndDay : PopupBase
{

    [SerializeField]
    RectTransform container;

    List<ResourceView> Views = new List<ResourceView>();

    public void Init(List<ResourceAmount> resources)
    {
        foreach (var r in resources) {
            var view = PoolManager.Spawn<ResourceView>("ResourceViewEndDay", container);
            view.Init(r.ID, r.Amount);
            Views.Add(view);
        }
        base.Init();

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
