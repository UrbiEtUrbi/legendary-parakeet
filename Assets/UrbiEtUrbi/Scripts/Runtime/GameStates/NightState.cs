using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightState : GameState
{

    [SerializeField]
    float InitialDelay;



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
    PopupBase InfoPopup, UpgradesPopup;


    bool isInside = true;

    bool IsPrepping, IsDefending;

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
                //TODO remove these magic numbers
               enemy.transform.rotation = default;
               enemy.Init(1, 1, 2, TheGame.Instance.Tower, sp.x > 0 ? 1.51f : -2.38f);
               enemyCount++;
               currentWaveEnemyCount++;
               yield return new WaitForSeconds(wave.singleDelay);
            }
        }
    }

    public void RemoveEnemy()
    {
        enemyCount--;

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

    public void SwitchView()
    {
        isInside = !isInside;
        UpdateState();
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
        Player.gameObject.SetActive(isInside);
    }


    public override void OnEndStage()
    {
        if (IsPrepping)
        {
            IsPrepping = false;
            IsDefending = true;
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