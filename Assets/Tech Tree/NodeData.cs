using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu, System.Serializable]
public class NodeData : ScriptableObject
{
    public List<NodeData> children;
    public bool IsLeaf { get => children == null; }

    public string techName = "new Technology";
    public int height;
    public bool available = false;

    [SerializeField] public Resource[] resources;
    [SerializeField] public int[] resourceCosts;

    public void Buy()
    {
        foreach (NodeData node in children)
        {
            node.Unblock();
        }
        available = false;
    }

    public void Unblock()
    {
        available = true;
    }
}
