using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TechTreeButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI nameText;
    [System.NonSerialized]
    NodeData nodeData;
    [SerializeField] bool available;

    [HideInInspector, SerializeField] private TechTree tree; // Need a reference so the button can check if we're in tree mode.

    [HideInInspector, SerializeField] public NodeData NodeData { get { return nodeData; } }

    /// <summary>
    /// Clones Node Data Preset
    /// </summary>
    /// <param name="_nodeData">Node Data Preset</param>
    /// <param name="techTree">Reference to TechTree</param>
    public void Setup(NodeData _nodeData, TechTree techTree)
    {

        nodeData = _nodeData;
        nameText.text = nodeData.techName;
        
        tree = techTree;
    }

   
    public void HandlePointerClick()
    {
        Debug.Log(nodeData.name);
        tree.Select(this);
    }

    public void BuyTech(){

        nodeData.Buy();
        tree.UpdateAll();
        
    }

}
