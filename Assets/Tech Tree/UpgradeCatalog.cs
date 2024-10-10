using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
using UnityEditor;
#endif

[CreateAssetMenu(fileName ="Upgrades", menuName = "Nightfall/Upgrades")]
public class UpgradeCatalog : ScriptableObject
{
    [EditorButton(nameof(GetValues))]
    public List<NodeData> Upgrades;


    public bool IsBought(string name, int level)
    {
        var tech = Upgrades.Find(x => x.name == name);
        if (tech != null)
        {
            return tech.CurrentLevel >= level;
        }
        return false;

    }

    public bool IsBought(UpgradeRequirement req)
    {
        return IsBought(req.NodeData.name, req.Level);
    }

    public bool IsAvailable(NodeData nodeData)
    {

        foreach (var req in nodeData.GetNextUpgrade().Required)
        {
            if (!IsBought(req))
            {
                return false;
            }
        }
        return true;
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
       var assets = UnityEditor.AssetDatabase.FindAssets("t:nodedata");

        int count = Upgrades.Count;
        Upgrades = new();
        foreach (var id in assets) {
            Upgrades.Add(UnityEditor.AssetDatabase.LoadAssetAtPath<NodeData>(UnityEditor.AssetDatabase.GUIDToAssetPath(id)));
        }

        if (count != Upgrades.Count)
        {
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
        }
        
    }




    private string sheetURL = "https://docs.google.com/spreadsheets/d/1YVosnRaqI6DFXpnsoZZSn_Q7vpeHJw8sWJHXFP9NGsM/pub?output=csv";

    public void GetLeaderboard()
    {

        EditorCoroutineUtility.StartCoroutine(FetchLeaderboard(),this);
      
    }

    IEnumerator FetchLeaderboard()
    {
        UnityWebRequest www = UnityWebRequest.Get(sheetURL);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string sheetData = www.downloadHandler.text;
            Parse(sheetData);
        }

    }

    void Parse(string data)
    {


        List<Resource> resources = new();
        var assets = UnityEditor.AssetDatabase.FindAssets("t:resource");

        int count = Upgrades.Count;
        Upgrades = new();
        foreach (var id in assets)
        {
            resources.Add(AssetDatabase.LoadAssetAtPath<Resource>(AssetDatabase.GUIDToAssetPath(id)));
        }


        OnValidate();
        string[] rows = data.Split('\n');
        var csvData = new CSVData();
        csvData.Parse(rows);

        foreach (var Upgrade in Upgrades)
        {
            Upgrade.Levels.Clear();
            foreach (var row in csvData.Rows)
            {
                if (row.Get("ID") == Upgrade.name)
                {
                    if (row.GetInt("Level") == 0)
                    {
                        Upgrade.DefaultValue = row.GetFloat("Value");
                    }
                    else
                    {
                        var level = new UpgradeLevel();


                        //Upgrade requirements
                        level.Required = new();
                        var upgradeRequirements = row.Get("Upgrade Requirements");
                        if (!string.IsNullOrEmpty(upgradeRequirements)){


                            var split = upgradeRequirements.Split(';');
                            foreach (var s in split)
                            {
                                var reqData = s.Split(':');

                                level.Required.Add(new UpgradeRequirement
                                {
                                    NodeData = Upgrades.Find(x => x.name == reqData[0]),
                                    Level = int.Parse(reqData[1])
                                }) ;
                            }
                        }


                        //upgrade cost
                        var cost = row.Get("Cost");
                        level.ResourceCost = new();
                        if (!string.IsNullOrEmpty(cost))
                        {
                            var split = cost.Split(';');
                            foreach (var s in split)
                            {
                                var costData = s.Split(':');
                                level.ResourceCost.Add(new ResourceCost
                                {
                                    Resource = resources.Find(x => x.name == costData[0]),
                                    Amount = int.Parse(costData[1])
                                });
                            }
                        }

                        // Requirement cost
                        var reqCost = row.Get("Requirement");
                        level.Requirement = new();
                        if (!string.IsNullOrEmpty(reqCost))
                        {
                            var split = reqCost.Split(';');
                            foreach (var s in split)
                            {
                                var costData = s.Split(':');
                                level.Requirement.Add(new ResourceCost
                                {
                                    Resource = resources.Find(x => x.name == costData[0]),
                                    Amount = int.Parse(costData[1])
                                });
                            }
                        }

                        level.Value = row.GetFloat("Value");
                        Upgrade.Levels.Add(level);
                    }
                }

            }

            EditorUtility.SetDirty(Upgrade);
            AssetDatabase.SaveAssetIfDirty(Upgrade);
        }
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssetIfDirty(this);
        Debug.Log("upgrades imported");

    }
#endif
    public void GetValues()
    {
#if UNITY_EDITOR
        GetLeaderboard();

#endif
    }

}

public class CSVData
{


    public List<CSVRow> Rows = new();

    Dictionary<string, int> Header = new();

    public void Parse(string[] rows)
    {
        var header = rows[0].Split(',');
        for (int i = 0; i < header.Length; i++)
        {
           
            Header.Add(header[i].Trim(), i);
        }
        for (int i = 1; i < rows.Length; i++)
        {
            var row = new CSVRow();
            row.Header = Header;
            var cols = rows[i].Split(',');
            foreach (var c in cols)
            {
                row.Data.Add(c);
            }
            Rows.Add(row);
        }
    }
}

public class CSVRow {

    public List<string> Data = new();
    public Dictionary<string, int> Header;

    public override string ToString()
    {
        return string.Join(",", Data);
    }

    public string Get(string field)
    {
        return Data[Header[field]];
    }

    public int GetInt(string field)
    {
        return int.Parse(Get(field));
    }

    public float GetFloat(string field)
    {
        return float.Parse(Get(field));
    }

}
