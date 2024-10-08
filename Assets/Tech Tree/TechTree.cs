using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;


public class TechTree : MonoBehaviour
{
    // Serializing this field makes changes persistent. Apparently.
    NodeData root;
    [SerializeField] GameObject nodePrefab;
    [SerializeField] GameObject columnPrefab;

    [SerializeField] CostDisplay costDisplayPrefab;

    [SerializeField] TextMeshProUGUI techNameText;
    [SerializeField] TMP_Text descriptionNameText;
    [SerializeField] HorizontalLayoutGroup costsLayout;

    [SerializeField] public bool treeMode;
    [SerializeField] bool update;

    List<VerticalLayoutGroup> columns = new List<VerticalLayoutGroup>();
    List<GameObject> generatedNodes = new List<GameObject>();
    [SerializeField] List<GameObject> generatedCosts = new List<GameObject>();
    [SerializeField] Transform row;
    [HideInInspector, SerializeField] TechTreeButton selectedTech;
    /// <summary>
    /// Correlates Node Data Presets with in-scene buttons, for purpose of linking buttons with children
    /// </summary>
    Dictionary<NodeData, GameObject> ButtonLookup;

    float rectWidth;

    public int Height { get; private set; }


    public void Init(NodeData nodeData)
    {
        root = nodeData;
        update = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (update)
        {
            Execute();
            update = false;
        }
      
    }

    public void Select(TechTreeButton ttb)
    {


        ClearReadout();
        PopulateReadout(ttb.NodeData);
        selectedTech = ttb;
    }

    public void TryBuy()
    {
        if (selectedTech == null) return;
        NodeData node = selectedTech.NodeData;
        for (int i = 0; i < node.resourceCosts.Length; i++)
        {
            CostDisplay costDisplay = Instantiate(costDisplayPrefab, costsLayout.transform);
            costDisplay.icon.sprite = node.resources[i].Icon;
            costDisplay.costText.text = node.resourceCosts[i].ToString();

            generatedCosts.Add(costDisplay.gameObject);

            // Grey Out Costs for which the player does not have enough of the resource
            if (TheGame.Instance == null) selectedTech.BuyTech();
            else if (TheGame.Res.GetResourceData(node.resources[i].ID, out var resAmount))
            {
                bool canAfford = true;
                (Resource resource, int amount) = resAmount;
                Debug.Log("Successfully got resource data for resource");
                if (amount < node.resourceCosts[i])
                {
                    Debug.Log($"Not enough of {node.resources[i].name}");
                    costDisplay.icon.color = new Color(0.6f, 0.6f, 0.6f, 0.6f);
                    canAfford = false;
                }
                if (canAfford)
                {
                    selectedTech.BuyTech();
                }
            }
        }
        UpdateAll();
    }

   

    void Execute()
    {
        //Height = CalculateHeight();
        //Debug.Log(Height);
        rectWidth = columnPrefab.GetComponent<RectTransform>().rect.width;
        foreach (VerticalLayoutGroup column in columns)
        {
            if (column != null) DestroyImmediate(column.gameObject);
        }
        ButtonLookup = new Dictionary<NodeData, GameObject>();
        columns.Clear();
        generatedNodes.Clear();
        if (treeMode) RegenerateTree();
        else RegenerateList();
    }

    private void RegenerateList()
    {
        if (root == null)
        {
            Debug.LogError("Tech Tree: ROOT NULL");
            return;
        }

        Stack<(NodeData node, int height)> stack = new Stack<(NodeData node, int height)>();
        stack.Push((root, 0));

        while (stack.Count > 0)
        {
            (NodeData node, int currentHeight) = stack.Pop();

            node.height = currentHeight;

            Debug.Log($"Visiting Node for {node.name} at Height {node.height}.");

            foreach (var child in node.children)
            {
                stack.Push((child, currentHeight + 1));
            }

            if (currentHeight == 0) continue;

            InstantiateAndPopulate(node, row.transform);
        }
        LinkAllNodes();
        UpdateAll();
    }

    void RegenerateTree()
    {
        if (root == null)
        {
            Debug.LogError("Tech Tree: ROOT NULL");
            return;
        }
        columns = new List<VerticalLayoutGroup>();
        // Can't believe C# supports tuples or whatever these're called
        Stack<(NodeData node, int height)> stack = new Stack<(NodeData node, int height)>();
        stack.Push((root, 0));

        while (stack.Count > 0)
        {
            (NodeData node, int currentHeight) = stack.Pop();

            node.height = currentHeight;

            Debug.Log($"Visiting Node for {node.name} at Height {node.height}. Currently {columns.Count} columns.");

            foreach (var child in node.children)
            {
                stack.Push((child, currentHeight + 1));
            }

            if (currentHeight == 0) continue;

            if (columns.Count < currentHeight) columns.Add(Instantiate(columnPrefab, new Vector3((rectWidth * currentHeight) - (rectWidth / 2), gameObject.transform.position.y), Quaternion.identity, transform).GetComponent<VerticalLayoutGroup>());
            InstantiateAndPopulate(node, columns[currentHeight - 1].transform);
        }
        LinkAllNodes();
        UpdateAll();
    }

    void LinkAllNodes()
    {
        Stack<(NodeData node, int height)> stack = new Stack<(NodeData node, int height)>();
        stack.Push((root, 0));

        while (stack.Count > 0)
        {
            (NodeData node, int currentHeight) = stack.Pop();

            node.height = currentHeight;

            Debug.Log($"Visiting Node for {node.name} at Height {node.height}.");

            foreach (var child in node.children)
            {
                stack.Push((child, currentHeight + 1));
                if (node == root) continue;
                // Find the instance version of NodeData "node." Add instance version of each child of Node
                ButtonLookup[node].GetComponent<TechTreeButton>().NodeData.children.Add(ButtonLookup[node].GetComponent<TechTreeButton>().NodeData);
            }
            if (currentHeight == 0) continue;
        }
    }

    public void UpdateAll()
    {
        Debug.Log("Updating List");
        foreach (GameObject node in generatedNodes)
        {
            node.GetComponent<TechTreeButton>().CheckUnlock();
            NodeData nodeData = node.GetComponent<TechTreeButton>().NodeData;
            Debug.Log("Checking Node " + nodeData.techName + ". Available: " + nodeData.available);
        }
    }

    public void PopulateReadout(NodeData node)
    {
        techNameText.text = node.techName;
        for (int i = 0; i < node.resourceCosts.Length; i++)
        {
            CostDisplay costDisplay = Instantiate(costDisplayPrefab, costsLayout.transform).GetComponent<CostDisplay>();
            costDisplay.icon.sprite = node.resources[i].Icon;
            costDisplay.costText.text = node.resourceCosts[i].ToString();
            descriptionNameText.text = node.Description;
            generatedCosts.Add(costDisplay.gameObject);

            // Grey Out Costs for which the player does not have enough of the resource
            if (TheGame.Instance == null) continue;
            if (TheGame.Res.GetResourceData(node.resources[i].ID, out var resAmount))
            {
                (Resource resource, int amount) = resAmount;
                Debug.Log("Successfully got resource data for resource");
                if (amount < node.resourceCosts[i])
                {
                    Debug.Log($"Not enough of {node.resources[i].name}");
                    costDisplay.icon.color = new Color(0.6f, 0.6f, 0.6f, 0.6f);
                }
            }
        }
        UpdateAll();
    }

    public void ClearReadout()
    {
        foreach (GameObject cost in generatedCosts)
        {
            DestroyImmediate(cost);
        }
        generatedCosts.Clear();
    }

    void InstantiateAndPopulate(NodeData nodeData, Transform parent)
    {
        GameObject node = Instantiate(nodePrefab, parent);
        node.GetComponent<TechTreeButton>().Setup(nodeData, this);
        generatedNodes.Add(node);
        ButtonLookup.Add(nodeData, node);
    }
}
