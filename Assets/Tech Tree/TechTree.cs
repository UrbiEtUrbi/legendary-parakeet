using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;


public class TechTree : MonoBehaviour
{
    [SerializeField] TechTreeButton nodePrefab;
    [SerializeField] GameObject columnPrefab;

    [SerializeField] CostDisplay costDisplayPrefab;

    [SerializeField] TextMeshProUGUI techNameText;
    [SerializeField] TMP_Text descriptionNameText;
    [SerializeField] HorizontalLayoutGroup costsLayout;

    [SerializeField] bool update;

    List<VerticalLayoutGroup> columns = new List<VerticalLayoutGroup>();
    List<TechTreeButton> generatedNodes = new List<TechTreeButton>();
    [SerializeField] List<GameObject> generatedCosts = new List<GameObject>();
    [SerializeField] Transform row;
    [HideInInspector, SerializeField] TechTreeButton selectedTech;

    float rectWidth;

    public int Height { get; private set; }



    private void OnEnable()
    {
       
        Execute();
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
        var nextLevel = node.GetNextUpgrade();

        bool canBuy = true;
        for (int i = 0; i < nextLevel.ResourceCost.Count; i++)
        {
            canBuy &= TheGame.Res.CanChange(nextLevel.ResourceCost[i].GetAmount());

        }
        if (canBuy)
        {

            for (int i = 0; i < nextLevel.ResourceCost.Count; i++)
            {
               TheGame.Res.Change(nextLevel.ResourceCost[i].GetAmount());

            }
            selectedTech.BuyTech();
            UpdateAll();
        }
        
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
        columns.Clear();
        generatedNodes.Clear();
        RegenerateList();
    }

    private void RegenerateList()
    {

        foreach (var tech in TheGame.Instance.Techs.Upgrades)
        {

            if (tech.Levels.Count == 0)
            {
                continue;
            }
            bool hasRequirements = true;

            foreach (var req in tech.GetNextUpgrade().Required)
            {
                hasRequirements &= TheGame.Instance.Techs.IsBought(req);
            }

            

            if (hasRequirements || tech.CurrentLevel > 0)
            {
                
               InstantiateAndPopulate(tech, row.transform);
            }
        }

        
        
        UpdateAll();
    }
 

    public void UpdateAll()
    {
        foreach (TechTreeButton node in generatedNodes)
        {
            if (node.NodeData.IsComplete)
            {
                node.transform.SetAsLastSibling();
            }
        }
    }

    public void PopulateReadout(NodeData node)
    {
        techNameText.text = node.techName;
        var nextLevel = node.GetNextUpgrade();
        for (int i = 0; i < nextLevel.ResourceCost.Count; i++)
        {
            CostDisplay costDisplay = Instantiate(costDisplayPrefab, costsLayout.transform).GetComponent<CostDisplay>();
            costDisplay.icon.sprite = nextLevel.ResourceCost[i].Resource.Icon;
            costDisplay.costText.text = nextLevel.ResourceCost[i].Amount.ToString();
            descriptionNameText.text = node.Description;
            generatedCosts.Add(costDisplay.gameObject);

            // Grey Out Costs for which the player does not have enough of the resource
            if (TheGame.Instance == null) continue;
            if (TheGame.Res.CanChange(nextLevel.ResourceCost[i].GetAmount()))
            {
                Debug.Log("Successfully got resource data for resource");
            }
        }
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
        TechTreeButton node = Instantiate(nodePrefab, parent);
        node.Setup(nodeData, this);
        generatedNodes.Add(node);
    }
}
