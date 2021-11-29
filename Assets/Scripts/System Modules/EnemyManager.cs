using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];
    public float TimeBetweenWaves => timeBetweenWaves;

    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float timeBetweenWaves = 1f;
    [SerializeField] private int minEnemyAmount = 4;
    [SerializeField] private int maxEnemyAmount = 10;

    [SerializeField] private IntEventChannelSO updateWaveEventSO;
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
        updateWaveEventSO.RaiseEvent(waveNumber);
    }

    private void RandomlySpawnEnemies()
    {
        currentEnemyAmount = enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / 3, maxEnemyAmount);

        for (int i = 0; i < enemyAmount; i++)
        {
            ObjectPoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);
        }
    }

    private void RemoveFromList()
    {
        currentEnemyAmount--;
        if (currentEnemyAmount <= 0)
        {
            updateWaveEventSO.RaiseEvent(++waveNumber);
        }
    }
}