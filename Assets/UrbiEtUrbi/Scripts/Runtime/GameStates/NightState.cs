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
    WaveCollection WaveCollection;

    [SerializeField]
    GameObject musicPrep, musicNight;

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

    [SerializeField]
    int baseWaveCount;

    [SerializeField]
    float waveCountPerRoundIncrease;


    bool isInside = true;

    bool IsPrepping, IsDefending;

    int TotalEnemies;
    int enemiesKilled;


    public List<Enemy> Enemies = new();



    [SerializeField]
    GameObject buttonSkip;



    List<int> freedSorting = new();
    int maxLayer;

    List<Wave> currentWaves;

    
    private void Start()
    {
        IsPrepping = true;
        IsDefending = false;
        enemyCount = 0;
        allEnemiesSpawned = false;
        isInside = true;
      
        UpdateState();
        PrimeTween.Tween.Delay(0.2f*Duration, () => buttonSkip.gameObject.SetActive(true));
    }



    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(InitialDelay);
        while (true)
        {

            if (waveIndex >= currentWaves.Count)
            {
                allEnemiesSpawned = true;
                EndNight();
                yield break;
            }
            var wave = currentWaves[waveIndex];


            while (wave.HasEnemies)
            {


                int enemiesToSpawn = Mathf.Min(Random.Range(1, 10), wave.Count);
                
                while (true)
                {


                    var sp = GetSpawnPosition();
                    List<string> enemyTypes = new List<string>();
                    if (wave.Swarm > 0)
                    {
                        enemyTypes.Add("WalkingEnemy");
                    }if (wave.Zeppelin > 0)
                    {
                        enemyTypes.Add("Blimp");
                    }if (wave.Car > 0)
                    {
                        enemyTypes.Add("Car");
                    }

                    var pick = enemyTypes[Random.Range(0, enemyTypes.Count)];
                    Enemy enemy = default;
                    switch (pick)
                    {
                        case "Car":
                            enemy = PoolManager.Spawn<ShootingEnemy>(pick, transform, sp);
                            wave.Car--;
                            break;
                        case "Blimp":
                            wave.Zeppelin--;
                            enemy = PoolManager.Spawn<ShootingEnemy>(pick, transform, sp);
                            break;


                        case "WalkingEnemy":
                            wave.Swarm--;
                            enemy = PoolManager.Spawn<WalkingEnemy>(transform, sp);
                            break;

                    }



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
                    enemy.transform.position = sp;
                    enemy.Init(2, TheGame.Instance.Tower, attackTarget);
                    enemyCount++;
                    Enemies.Add(enemy);
                    yield return new WaitForSeconds(0.05f);
                    enemiesToSpawn--;
                    if (enemiesToSpawn <= 0)
                    {
                        break;
                    }
                }
               

                yield return new WaitForSeconds(2f);
            }
            waveIndex++;


        }
    }

    public void TryRepair()
    {

        if (TheGame.Res.CanChange(0, -10)){
            TheGame.Res.Change(0, -10);
            TheGame.Instance.Tower.ChangeHealth(0.2f);
            SoundManager.Instance.Play("repair");
        }
    }

    public void RemoveEnemy(Enemy e)
    {
        enemyCount--;
        enemiesKilled++;
        Enemies.Remove(e);
        freedSorting.Add(e.GetLayer());
        TheGame.Instance.GameCycleManager.SetProgress(1 - (float)enemiesKilled/(float)TotalEnemies);
        EndNight();
    }

    public void EndNight()
    {
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
        if (UnityEngine.InputSystem.Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SwitchView();
        }
    }

    public void StartAttack()
    {
        buttonSkip.SetActive(false);
        TheGame.Instance.GameCycleManager.SkipTime();
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
            buttonSkip.SetActive(false);
            musicPrep.gameObject.SetActive(false);
            musicNight.gameObject.SetActive(true);
            IsPrepping = false;
            IsDefending = true;
            TheGame.Instance.GameCycleManager.DebugLabel.text = "Enemies:";

            currentWaves = WaveCollection.GetWave(baseWaveCount + Mathf.FloorToInt(TheGame.Instance.RoundNumber * waveCountPerRoundIncrease));
       //     Debug.Log(currentWaves.Count);
            waveIndex = 0;
            foreach (var w in currentWaves)
            {
                TotalEnemies += w.Car;
                TotalEnemies += w.Swarm;
                TotalEnemies += w.Zeppelin;
            }
//            Debug.Log(TotalEnemies);
            TheGame.Instance.GameCycleManager.SetProgress(1);
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




