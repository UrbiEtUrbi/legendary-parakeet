using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheGame : ControllerLocal
{

    public static TheGame Instance
    {
        get => m_Instance;
    }
    private static TheGame m_Instance;


    [HideInInspector]
    public bool IsGameOver;

    [HideInInspector]
    public bool IsGamePlaying;




    [field: SerializeField]
    public ControllerRespawn ControllerRespawn { private set; get; }

    [field: SerializeField]
    public ControllerPickups ControllerPickups { private set; get; }

    [field: SerializeField]
    public ControllerEntities ControllerEntities { private set; get; }

    [field: SerializeField]
    public GameCycleManager GameCycleManager { private set; get; }

    [field: SerializeField]
    public ControllerResources ControllerResources { private set; get; }
    [field: SerializeField]
    public Canvas Canvas { private set; get; }



    private void Awake()
    {
        m_Instance = this;
    }

    public override void Init()
    {
        base.Init();
        GameCycleManager.EnterState(GameStateType.Day);
    }

    public void Save()
    {
        ControllerLoadingScene.Instance.Save();

    }
}



