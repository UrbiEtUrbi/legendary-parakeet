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


    [SerializeField]
    PopupBase gameover;

    [SerializeField]
    NodeData TowerHealth;


    public void GameOver()
    {
        ControllerLoadingScene.Instance.Remove();
        ControllerGameFlow.Instance.LoadNewScene(1);
    }

    public void OnGameOver()
    {
        GameCycleManager.exit();
        gameover.Show();
    }

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


        foreach (var resSave in ControllerLoadingScene.Instance.SaveData.resourceSaves)
        {
            ControllerResources.Change(resSave,false);
        }
        base.Init();
        GameCycleManager.EnterState(GameStateType.Day);
        m_Tower = new Tower();
        m_Tower.SetInitialHealth(TowerHealth.GetValue());
    }

    public void Save()
    {
        ControllerLoadingScene.Instance.Save();

    }
    public void CheatRes()
    {
        foreach (var res in Res.ResourceCollection)
        {
            ControllerResources.Change(res, 100);
        }
    }

    public void BuyUpgrade(NodeData upgrade) 
    {
        switch (upgrade.name)
        {
            case "TowerHealth":
                Tower.SetMaxHealth(upgrade.GetValue());
                break;

        }

    }
}



