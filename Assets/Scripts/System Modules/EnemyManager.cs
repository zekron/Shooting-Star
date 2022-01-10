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

    private bool spawnEnemy = true;
    private int waveNumber = 0;
    private int enemyAmount;
    private int currentEnemyAmount;

    private List<GameObject> enemyList;

    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];
    public float TimeBetweenWaves => timeBetweenWaves;
    public bool SpawnEnemy { get => spawnEnemy; set => spawnEnemy = value; }

    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>(maxEnemyAmount);
    }

    private void OnEnable()
    {
        animationClipFinishedEventSO.OnEventRaised += RandomlySpawnEnemies;
        enemyDestroyEventSO.OnEventRaised += RemoveFromList;
    }

    private void OnDisable()
    {
        animationClipFinishedEventSO.OnEventRaised -= RandomlySpawnEnemies;
        enemyDestroyEventSO.OnEventRaised -= RemoveFromList;
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
        if (!spawnEnemy) return;
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

    public void SpawnEnemyNow()
    {
#if DEBUG_MODE
        enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / bossWave, maxEnemyAmount);
        currentEnemyAmount += enemyAmount;

        for (int i = 0; i < enemyAmount; i++)
        {
            ObjectPoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);
        }
#endif
    }
    public void SpawnBossNow()
    {
#if DEBUG_MODE
        currentEnemyAmount += 1;

        ObjectPoolManager.Release(bossPrefabs[Random.Range(0, bossPrefabs.Length)]);
    }
#endif
}