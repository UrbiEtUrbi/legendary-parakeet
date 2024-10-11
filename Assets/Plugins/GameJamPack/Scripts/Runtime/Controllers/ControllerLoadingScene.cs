using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ControllerLoadingScene : MonoBehaviour
{

    [SerializeField, SceneDetails]
    SerializedScene Scene;

    public static ControllerLoadingScene Instance;


    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {

        Load();
        ControllerGameFlow.Instance.LoadNewScene(Scene.BuildIndex);
    }



    
    public SaveData SaveData;
    public bool HasSave => SaveData != null;

    const string SaveStr = "save_str";

    public void Save(SaveData saveData)
    {
        PlayerPrefs.SetString(SaveStr, JsonUtility.ToJson(saveData));
        PlayerPrefs.Save();
    }

    public void Save()
    {
        PlayerPrefs.SetString(SaveStr, JsonUtility.ToJson(SaveData));
        PlayerPrefs.Save();
    }

    public void Remove()
    {

        PlayerPrefs.DeleteKey(SaveStr);
    }

    public void Load()
    {
       var str =  PlayerPrefs.GetString(SaveStr,string.Empty);
        if (!string.IsNullOrEmpty(str))
        {
            SaveData = JsonUtility.FromJson<SaveData>(str);
        }
        else
        {
            SaveData = new();
        }
    }





}

[System.Serializable]
public class SaveData
{
    public int MaxHp;
    public int CurrentHP;

    public List<ResourceAmount> resourceSaves = new();
    public List<TechSave> techSaves = new();

    public int CurrentRound;
    public GameStateType gameStateType;


    public void Tech(string name, int level)
    {
        var idx = techSaves.FindIndex(x => x.Name == name);

        if (idx != -1)
        {
            techSaves[idx].Level = level;
        } else{

            techSaves.Add(new TechSave
            {
                Level = level,
                Name = name
            }) ;
            }
    }

    public void Resource(ResourceAmount resourceAmount)
    {
        var idx = resourceSaves.FindIndex(x => x.ID == resourceAmount.ID);

        if (idx != -1)
        {
            resourceSaves[idx].Amount = resourceAmount.Amount;
        }
        else
        {

            resourceSaves.Add(new ResourceAmount(resourceAmount.ID, resourceAmount.Amount));
        }
    }




}

[System.Serializable]
public class TechSave
{
    public string Name;
    public int Level;

}

[System.Serializable]
public class ResourceAmount
{
    public int ID;
    public int Amount;

    public ResourceAmount(int id, int amount)
    {
        ID = id;
        Amount = amount;
    }

}

[System.Serializable]
public enum GameStateType
{
    Day,
    Night,
}

