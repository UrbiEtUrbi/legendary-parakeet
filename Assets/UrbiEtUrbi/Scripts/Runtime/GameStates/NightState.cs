using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NightState : GameState
{

    [SerializeField]
    float InitialDelay;


    [SerializeField]
    Bar HealthBar;

    [SerializeField]
    List<Wave> EnemyWaves;

    [SerializeField]
    List<BoxCollider2D> SpawnPositionGround;

    int waveIndex = 0;

    int enemyCount;

    bool allEnemiesSpawned;

    [SerializeField]
    List<GameObject> CameraExterior, CameraInterior;

    [SerializeField]
    Camera CameraInteriorCam, CameraExteriorCam;

    [SerializeField]
    PopupBase InfoPopup, UpgradesPopup;

    [SerializeField]
    InteriorController InteriorController;

    [SerializeField]
    Transform attackTarget;


    bool isInside = true;

    bool IsPrepping, IsDefending;

    int TotalEnemies;
    int enemiesKilled;


    public List<Enemy> Enemies = new();


    List<int> freedSorting = new();
    int maxLayer;

    
    private void Start()
    {
        IsPrepping = true;
        IsDefending = false;
        enemyCount = 0;
        allEnemiesSpawned = false;
        isInside = true;
      
        UpdateState();


    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(InitialDelay);
        while (true)
        {

            if (waveIndex >= EnemyWaves.Count)
            {
                allEnemiesSpawned = true;
                yield break;
            }
            var wave = EnemyWaves[waveIndex];
            waveIndex++;

            yield return new WaitForSeconds(wave.delay);
            int currentWaveEnemyCount = 0;

            while (currentWaveEnemyCount < wave.count)
            {

               var sp = GetSpawnPosition();


                

               var enemy =  PoolManager.Spawn<WalkingEnemy>(transform, sp);

                if (freedSorting.Count > 0)
                {
                    var idx = Random.Range(0, freedSorting.Count);
                    enemy.SetLayer(freedSorting[idx]);
                    freedSorting.RemoveAt(idx);
                }
                else
                {
                    enemy.SetLayer(maxLayer);
                    maxLayer++;
                }
                //TODO remove these magic numbers
               enemy.transform.rotation = default;
               enemy.Init(2, TheGame.Instance.Tower, attackTarget);
               enemyCount++;
               Enemies.Add(enemy);
               currentWaveEnemyCount++;
               yield return new WaitForSeconds(wave.singleDelay);
            }

            TheGame.Instance.GameCycleManager.SetProgress(1);
        }
    }

    public void RemoveEnemy(Enemy e)
    {
        enemyCount--;
        enemiesKilled++;
        Enemies.Remove(e);
        freedSorting.Add(e.GetLayer());
        TheGame.Instance.GameCycleManager.SetProgress(1 - (float)enemiesKilled/(float)TotalEnemies);
        if (allEnemiesSpawned && enemyCount <= 0)
        {
            TheGame.Instance.GameCycleManager.EnterState(GameStateType.Day);
        }
    }

    Vector3 GetSpawnPosition() {


        var idx = Random.Range(0, SpawnPositionGround.Count);
        var collider = SpawnPositionGround[idx];

        return collider.transform.position += new Vector3(Random.Range(-collider.size.x * 0.5f, collider.size.x * 0.5f),0,0);
    }

    public void SetHealth(float value)
    {
        HealthBar.SetValue(value);
    }

    public void SwitchView()
    {
        isInside = !isInside;
        InteriorController.Toggle(isInside);
        UpdateState();
    }

    private void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SwitchView();
        }
    }

    void UpdateState()
    {
        foreach (var ext in CameraExterior)
        {


            ext.SetActive(!isInside);
        }

        foreach (var inte in CameraInterior)
        {
            inte.SetActive(isInside);
        }
        TheGame.Instance.GameCycleManager.CurrentCamera = isInside ? CameraInteriorCam : CameraExteriorCam;
        Player.gameObject.SetActive(isInside);
    }


    public override void OnEndStage()
    {
        if (IsPrepping)
        {
            IsPrepping = false;
            IsDefending = true;
            TheGame.Instance.GameCycleManager.DebugLabel.text = "Enemies:";

            foreach (var w in EnemyWaves)
            {
                TotalEnemies += (int)w.count;
            }
            StartCoroutine(SpawnEnemy());
            return;
        }
        else if (IsDefending)
        {
            IsDefending = false;
            base.OnEndStage();
        }

        
       
    }


}


[System.Serializable]
public class Wave
{
    public float delay;
    public float singleDelay;
    public float count;

}


