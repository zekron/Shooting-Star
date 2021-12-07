using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];
    public float TimeBetweenWaves => timeBetweenWaves;

    [SerializeField] private bool spawnEnemy = true;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] bossPrefabs;
    [SerializeField] private float timeBetweenWaves = 1f;
    [SerializeField] private int bossWave = 5;
    [SerializeField] private int minEnemyAmount = 4;
    [SerializeField] private int maxEnemyAmount = 10;

    [SerializeField] private IntEventChannelSO updateWaveEventSO;
    [SerializeField] private IntEventChannelSO enemyLevelUpEventSO;
    [SerializeField] private VoidEventChannelSO enemyDestroyEventSO;
    [SerializeField] private VoidEventChannelSO animationClipFinishedEventSO;

    private int waveNumber = 1;
    private int enemyAmount;
    private int currentEnemyAmount;

    private List<GameObject> enemyList;

    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>();

        animationClipFinishedEventSO.OnEventRaised += RandomlySpawnEnemies;
        enemyDestroyEventSO.OnEventRaised += RemoveFromList;
    }

    private void Start()
    {
        //updateWaveEventSO.RaiseEvent(waveNumber);
        RandomlySpawnEnemies();
    }

    private void RandomlySpawnEnemies()
    {
        if (!spawnEnemy || GameManager.CurrentGameState != GameState.Playing) return;

        if (waveNumber % bossWave == 0)
        {
            ObjectPoolManager.Release(bossPrefabs[Random.Range(0, bossPrefabs.Length)]);
        }
        else
        {
            currentEnemyAmount = enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / bossWave, maxEnemyAmount);

            for (int i = 0; i < enemyAmount; i++)
            {
                ObjectPoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);
            }
        }
    }

    private void RemoveFromList()
    {
        currentEnemyAmount--;
        if (currentEnemyAmount <= 0)
        {
            if ((++waveNumber - bossWave) % bossWave == 1)
                enemyLevelUpEventSO.RaiseEvent(waveNumber);

            if (waveNumber % bossWave == 0)
                updateWaveEventSO.RaiseEvent(waveNumber);
            else
                RandomlySpawnEnemies();
        }
    }
}