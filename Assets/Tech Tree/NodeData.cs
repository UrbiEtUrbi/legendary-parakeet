using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu, System.Serializable]
public class NodeData : ScriptableObject
{
    

    public string techName = "new Technology";


    [System.NonSerialized]
    public int CurrentLevel;

    public float DefaultValue;
    public bool IsComplete => CurrentLevel >= Levels.Count;


    public string Description;



    [SerializeField]
    public List<UpgradeLevel> Levels = new();

    public float GetValue()
    {
        if (CurrentLevel == 0)
        {
            return DefaultValue;
        }
        else if (IsComplete)
        {
            return Levels[^1].Value;

        }
        {
            return Levels[Mathf.Min(CurrentLevel-1, Levels.Count)].Value;

        }
    }

    public UpgradeLevel GetNextUpgrade()
    {
        Debug.Log($"{name} {CurrentLevel} {Levels.Count-1}");
        return Levels[Mathf.Min(CurrentLevel, Levels.Count-1)];
    }

    public UpgradeLevel GetCurrentUpgrade()
    {

        if (CurrentLevel == 0)
        {
            return new UpgradeLevel
            {
                Value = DefaultValue
            };
        }
        return Levels[Mathf.Min(CurrentLevel-1, Levels.Count)];
    }

    public void Buy()
    {
        CurrentLevel++;
        TheGame.Instance.BuyUpgrade(this);
        ControllerLoadingScene.Instance.SaveData.Tech(name, CurrentLevel);
    }
 
}

[System.Serializable]
public class ResourceCost
{
    public Resource Resource;
    public int Amount;
    public ResourceAmount GetAmount()
    {
        return new ResourceAmount(Resource.ID, -Amount);
    }
}

[System.Serializable]
public class UpgradeLevel
{
    public List<UpgradeRequirement> Required;
    public List<ResourceCost> ResourceCost;
    public List<ResourceCost> Requirement;
    public float Value;
}

[System.Serializable]
public class UpgradeRequirement
{
    public NodeData NodeData;
    public int Level;

}

