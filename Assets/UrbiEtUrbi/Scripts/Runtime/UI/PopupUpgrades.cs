using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUpgrades : PopupBase
{
    int CurrentTab;

    [SerializeField]
    List<GameObject> Tabs;


    [SerializeField]
    NodeData repairTech;

    [SerializeField]
    GameObject RepairBlocker;

    public void SelectTab(int TabIndex)
    {
        CurrentTab = TabIndex;

        RepairBlocker.SetActive(repairTech.GetValue() == 0);


        for (int i = 0; i < Tabs.Count; i++)
        {
            Tabs[i].SetActive(i == TabIndex);
        }
    }
}
