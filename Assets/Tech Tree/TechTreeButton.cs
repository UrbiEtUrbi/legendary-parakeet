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
    [SerializeField] NodeData nodeData;
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
        Debug.Log($"Node Setup: {_nodeData.name}. Available: {_nodeData.available}");
        nodeData = ScriptableObject.CreateInstance<NodeData>();
        nodeData.children = new List<NodeData>();
        nodeData.name = _nodeData.name;
        nodeData.techName = _nodeData.techName;
        nodeData.available = _nodeData.available;
        Debug.Log($"Set node name to: {nodeData.name}");

        nameText.text = nodeData.techName;
        available = nodeData.available;
        
        tree = techTree;
    }

    public void HandlePointerEnter()
    {
        if (tree.treeMode)
        {
            throw new NotImplementedException("Tree Mode not implemented for TechTreeButton.cs");
        }
        else
        {
            //GetComponent<RectTransform>().transform.localPosition = new Vector2(GetComponent<RectTransform>().rect.position.x, GetComponent<RectTransform>().rect.position.y + 10);
            //tree.PopulateReadout(nodeData);
        }
    }

    public void HandlePointerExit()
    {
        if (tree.treeMode)
        {
            throw new NotImplementedException("Tree Mode not implemented for TechTreeButton.cs");
        }
        else
        {
            //tree.ClearReadout();
        }
    }

    public void HandlePointerClick()
    {
        if (tree.treeMode)
        {
            // buy straightaway
            throw new NotImplementedException("Tree Mode not implemented for TechTreeButton.cs");
        }
        else
        {

            tree.Select(this);
        }
    }

    public void BuyTech()
    {
        if (tree.treeMode)
        {

        }
        else
        {
            nodeData.Buy();
            tree.UpdateAll();
        }
    }

    public void CheckUnlock()
    {
        gameObject.SetActive(nodeData.available);
    }
}
