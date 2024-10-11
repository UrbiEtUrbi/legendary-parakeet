using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
using Unity.EditorCoroutines.Editor;
#endif

[CreateAssetMenu(menuName = "Nightfall/Waves", fileName ="Waves")]
public class WaveCollection : ScriptableObject
{
    [EditorButton(nameof(GetValues))]
    public List<Wave> Waves = new();

    public List<Wave> GetWave(int count)
    {
        var techCount = TheGame.Instance.Techs.Upgrades.Count(x => x.CurrentLevel > 0);
        var candiadates = Waves.FindAll(x => TheGame.Instance.RoundNumber >= x.MinRound && TheGame.Instance.RoundNumber <= x.MaxRound && techCount >= x.MinUpgrades).ToList();

        var waves = new List<Wave>();
        for (int i = 0; i < count; i++)
        {
            waves.Add(candiadates[Random.Range(0, candiadates.Count)].CreateWave(TheGame.Instance.RoundNumber));
        }
        return waves;
    }


    private string sheetURL = "https://docs.google.com/spreadsheets/d/1YVosnRaqI6DFXpnsoZZSn_Q7vpeHJw8sWJHXFP9NGsM/pub?gid=701586149&single=true&output=csv";

#if UNITY_EDITOR
    public void GetLeaderboard()
    {

        EditorCoroutineUtility.StartCoroutine(FetchLeaderboard(), this);

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
        Waves.Clear();

        string[] rows = data.Split('\n');
        var csvData = new CSVData();
        csvData.Parse(rows);

        foreach (var row in csvData.Rows)
        {

            var wave = new Wave();

            wave.Swarm = row.GetInt("Swarm");
            wave.Car = row.GetInt("Vehicle");
            wave.Zeppelin = row.GetInt("Zeppelin");
            wave.MinRound = row.GetInt("Minimum Night Number");
            wave.MaxRound = row.GetInt("Maximum Night Number");
            wave.MinUpgrades = row.GetInt("Minimum Tech Number");
            wave.Factor = row.GetFloat("Factor");
            Waves.Add(wave);

        }

       
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssetIfDirty(this);
        Debug.Log("waves imported");

    }
#endif
    public void GetValues()
    {
#if UNITY_EDITOR
        GetLeaderboard();

#endif
    }
}

[System.Serializable]
public class Wave
{
    public int Swarm;
    public int Zeppelin;
    public int Car;
    public int MinRound;
    public int MaxRound;
    public int MinUpgrades;
    public float Factor;

    public override string ToString()
    {
        return $"swarm: {Swarm} car: {Car} blimp: {Zeppelin}";
    }
    public bool HasEnemies
    {
        get
        {
            return Count > 0;
        }
    }

    public int Count => Swarm + Zeppelin + Car;

    public Wave CreateWave(int round)
    {
        return new Wave
        {
            Swarm = Mathf.Max(1,Mathf.RoundToInt(Factor *(round - MinRound)))*Swarm,
            Zeppelin = Mathf.Max(1,Mathf.RoundToInt(Factor * (round-MinRound)))*Zeppelin,
            Car = Mathf.Max(1, Mathf.RoundToInt(Factor * (round - MinRound)))*Car,

        };

    }

}
