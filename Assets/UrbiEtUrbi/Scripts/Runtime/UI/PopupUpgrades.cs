using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUpgrades : PopupBase
{
    int CurrentTab;

    [SerializeField]
    List<GameObject> Tabs;

    public void SelectTab(int TabIndex)
    {
        CurrentTab = TabIndex;
        for (int i = 0; i < Tabs.Count; i++)
        {
            Tabs[i].SetActive(i == TabIndex);
        }
    }
}
