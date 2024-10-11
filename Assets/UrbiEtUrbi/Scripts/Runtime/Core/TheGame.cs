using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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


    private Tower m_Tower;

    public Tower Tower => m_Tower;


    public UpgradeCatalog Techs;


    [field: SerializeField]
    public ControllerRespawn ControllerRespawn { private set; get; }

    [field: SerializeField]
    public ControllerPickups ControllerPickups { private set; get; }

    [field: SerializeField]
    public ControllerEntities ControllerEntities { private set; get; }

    [field: SerializeField]
    public GameCycleManager GameCycleManager { private set; get; }

    [field: SerializeField]
    public ControllerAttack ControllerAttack { private set; get; }

    [field: SerializeField]
    public ControllerResources ControllerResources { private set; get; }

    public static ControllerResources Res => Instance.ControllerResources;

    [field: SerializeField]
    public Canvas Canvas { private set; get; }

    [field: SerializeField]
    public ControllerInteractibles ControllerInteractibles { private set; get; }


    public int RoundNumber;

   



    private void Awake()
    {
        m_Instance = this;
    }

    public override void Init()
    {
        foreach (var tech in Techs.Upgrades)
        {
            var save = ControllerLoadingScene.Instance.SaveData.techSaves.Find(x => x.Name == tech.name);
            if (save != null)
            {
                tech.CurrentLevel = save.Level;
            }
        }

       
        base.Init();
        GameCycleManager.EnterState(GameStateType.Day);
        m_Tower = new Tower();
        m_Tower.SetInitialHealth(100);
    }

    public void Save()
    {
        ControllerLoadingScene.Instance.Save();

    }
}



