using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ControllerMainMenu : ControllerLocal
{


    [SerializeField, SceneDetails]
    SerializedScene Scene;



    public override void Init()
    {
        base.Init();
     
    }

    public void OnPlay()
    {
        ControllerGameFlow.Instance.LoadNewScene(Scene.BuildIndex);
    }

    private string URL = "https://script.google.com/macros/s/AKfycbyMVIethV1PdOpHzz0xo-BlIi_7MEI1kna2FXl3zV90ayGw-LlWxQqPccMs07jBTn9jhg/exec";


    public int Score;
    [EditorButton(nameof(Submit))]
    [EditorButton(nameof(GetLeaderboard))]
    public string Name;



    void Submit()
    {
        SubmitScore(Name, Score);
    }

    public void SubmitScore(string playerName, int score)
    {
        StartCoroutine(SendScore(playerName, score));
    }

    IEnumerator SendScore(string playerName, int score)
    {
        string jsonData = "{\"PlayerName\":\"" + playerName + "\",\"Score\":" + score + "}";

        UnityWebRequest www = new UnityWebRequest(URL, "POST");
        byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Score Submitted Successfully");
        }
    }

    private string sheetURL = "https://docs.google.com/spreadsheets/d/1YVosnRaqI6DFXpnsoZZSn_Q7vpeHJw8sWJHXFP9NGsM/pub?output=csv";

    public void GetLeaderboard()
    {
        StartCoroutine(FetchLeaderboard());
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
            ParseAndDisplayLeaderboard(sheetData);
        }
    }

    void ParseAndDisplayLeaderboard(string data)
    {
        string[] rows = data.Split('\n');
        Debug.Log(data);
        foreach (string row in rows.Skip(1)) // Skip the header row
        {
            string[] columns = row.Split(',');
            string playerName = columns[0];
            string score = columns[1];

            Debug.Log("Player: " + playerName + " Score: " + score);
            // Here you can update your UI to display the leaderboard
        }
    }






}
