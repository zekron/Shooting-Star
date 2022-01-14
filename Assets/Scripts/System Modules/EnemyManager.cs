using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
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

    private bool needSpawnEnemy = true;
    private int waveNumber = 0;
    private int enemyAmount;
    private int currentEnemyAmount;

    private List<GameObject> enemyList;

    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];
    public float TimeBetweenWaves => timeBetweenWaves;

    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>(maxEnemyAmount);
    }

    private void OnEnable()
    {
        animationClipFinishedEventSO.OnEventRaised += RandomlySpawnEnemies;
        enemyDestroyEventSO.OnEventRaised += RemoveFromList;

#if DEBUG_MODE
        spawnEnemyNowEvent.OnEventRaised += SpawnEnemyNow;
        spawnBossNowEvent.OnEventRaised += SpawnBossNow;
        needSpawnEnemyEvent.OnEventRaised += SetNeedSpawnEnemy;
#endif
    }

    private void OnDisable()
    {
        animationClipFinishedEventSO.OnEventRaised -= RandomlySpawnEnemies;
        enemyDestroyEventSO.OnEventRaised -= RemoveFromList;

#if DEBUG_MODE
        spawnEnemyNowEvent.OnEventRaised -= SpawnEnemyNow;
        spawnBossNowEvent.OnEventRaised -= SpawnBossNow;
        needSpawnEnemyEvent.OnEventRaised -= SetNeedSpawnEnemy;
#endif
    }

    private void Start()
    {
        //updateWaveEventSO.RaiseEvent(waveNumber);
        updateWaveEventSO.RaiseEvent(waveNumber++);
        //RandomlySpawnEnemies();
    }

    private void RandomlySpawnEnemies()
    {
#if DEBUG_MODE
        if (!needSpawnEnemy) return;
#endif
        if (GameManager.Instance.CurrentGameState != GameState.Playing) return;

        if (waveNumber % bossWave == 0)
        {
            currentEnemyAmount = enemyAmount = 1;

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
            ++waveNumber;

            if ((waveNumber - bossWave) % bossWave == 1)
                enemyLevelUpEventSO.RaiseEvent(waveNumber);

            if (waveNumber % bossWave == 0)
                updateWaveEventSO.RaiseEvent(waveNumber);
            else
                RandomlySpawnEnemies();
        }
    }

    #region Debug
#if DEBUG_MODE
    [Header("DEBUG")]
    [SerializeField] private BooleanEventChannelSO needSpawnEnemyEvent;
    [SerializeField] private VoidEventChannelSO spawnEnemyNowEvent;
    [SerializeField] private VoidEventChannelSO spawnBossNowEvent;
    private void SpawnEnemyNow()
    {
        enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / bossWave, maxEnemyAmount);
        currentEnemyAmount += enemyAmount;

        for (int i = 0; i < enemyAmount; i++)
        {
            ObjectPoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);
        }
    }
    private void SpawnBossNow()
    {
        currentEnemyAmount += 1;

        ObjectPoolManager.Release(bossPrefabs[Random.Range(0, bossPrefabs.Length)]);
    }
    private void SetNeedSpawnEnemy(bool value)
    {
        needSpawnEnemy = value;
    }
#endif
    #endregion
}