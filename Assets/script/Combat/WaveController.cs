/*using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawnData
    {
        public BattleUnit enemyPrefab;
        public Transform spawnPoint;
    }

    [System.Serializable]
    public class WaveData
    {
        public List<EnemySpawnData> enemies = new List<EnemySpawnData>();
    }

    [Header("Refs")]
    public TurnBattleManager battleManager;

    [Header("Waves")]
    public List<WaveData> waves = new List<WaveData>();

    [Header("Runtime")]
    public int currentWaveIndex = 0;
    public List<BattleUnit> currentEnemies = new List<BattleUnit>();



    void Start()
    {
        StartWave();
    }



    void Update()
    {
        if (currentEnemies.Count == 0)
        {
            return;
        }

        bool allDead = true;

        for (int i = 0; i < currentEnemies.Count; i++)
        {
            if (currentEnemies[i] != null && currentEnemies[i].currentHP > 0)
            {
                allDead = false;
                break;
            }
        }

        if (allDead)
        {
            StartNextWave();
        }
    }



    public void StartWave()
    {
        currentEnemies.Clear();

        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("All waves cleared");
            return;
        }

        WaveData wave = waves[currentWaveIndex];

        for (int i = 0; i < wave.enemies.Count; i++)
        {
            EnemySpawnData data = wave.enemies[i];

            if (data.enemyPrefab == null || data.spawnPoint == null)
            {
                continue;
            }

            BattleUnit newEnemy = Instantiate(data.enemyPrefab, data.spawnPoint.position, data.spawnPoint.rotation);
            currentEnemies.Add(newEnemy);
        }

        if (battleManager != null)
        {
            battleManager.RefreshBattleUnits();
        }

        Debug.Log("Wave " + (currentWaveIndex + 1) + " started");
    }



    public void StartNextWave()
    {
        currentWaveIndex++;

        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("Battle finished");
            return;
        }

        StartWave();
    }

}*/