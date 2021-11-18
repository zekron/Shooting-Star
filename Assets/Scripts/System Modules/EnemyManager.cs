using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];
    public int WaveNumber => waveNumber;
    public float TimeBetweenWaves => timeBetweenWaves;

    [SerializeField] private bool spawnEnemy = true;
    [SerializeField] private GameObject waveUI;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float timeBetweenSpawns = 1f;
    [SerializeField] private float timeBetweenWaves = 1f;
    [SerializeField] private int minEnemyAmount = 4;
    [SerializeField] private int maxEnemyAmount = 10;

    private int waveNumber = 1;
    private int enemyAmount;

    private List<GameObject> enemyList;

    private WaitForSeconds waitTimeBetweenSpawns;
    private WaitForSeconds waitTimeBetweenWaves;

    private WaitUntil waitUntilNoEnemy;

    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>();
        waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
        waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
        waitUntilNoEnemy = new WaitUntil(() => enemyList.Count == 0);
    }

    IEnumerator Start()
    {
        while (spawnEnemy)
        {
            waveUI.SetActive(true);

            yield return waitTimeBetweenWaves;

            waveUI.SetActive(false);

            yield return StartCoroutine(nameof(RandomlySpawnCoroutine));
        }
    }

    IEnumerator RandomlySpawnCoroutine()
    {
        enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / 3, maxEnemyAmount);

        for (int i = 0; i < enemyAmount; i++)
        {
            enemyList.Add(ObjectPoolManager.Release(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]));

            yield return waitTimeBetweenSpawns;
        }

        yield return waitUntilNoEnemy;

        waveNumber++;
    }

    public void RemoveFromList(GameObject enemy) => enemyList.Remove(enemy);
}