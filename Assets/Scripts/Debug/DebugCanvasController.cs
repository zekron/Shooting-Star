using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugCanvasController : MonoBehaviour
{
    [SerializeField] private Text fpsText;
    [SerializeField] private Button spawnEnemyButton;
    [SerializeField] private Toggle spawnEnemyToggle;

    WaitForSeconds waitForRefreshFPS = new WaitForSeconds(0.5f);

    int frames = 0;
    private float updateInterval = 0.05f;
    private float lastUpdateTime;
    private float fps;
    private float frameDeltaTime;

    #region Unity Functions
    private void OnEnable()
    {
        spawnEnemyToggle.isOn = EnemyManager.Instance.SpawnEnemy;

        spawnEnemyToggle.onValueChanged.AddListener(SetSpawnEnemy);
        spawnEnemyButton.onClick.AddListener(SpawnEnemy);
    }

    private void OnDisable()
    {
        spawnEnemyToggle.onValueChanged.RemoveListener(SetSpawnEnemy);
        spawnEnemyButton.onClick.RemoveListener(SpawnEnemy);
    }

    void Start()
    {
    }

    void Update()
    {
#if DEBUG_MODE
        CheckFPS();
#endif
    }
    #endregion

    private void SpawnEnemy()
    {
        EnemyManager.Instance.SpawnEnemyNow();
    }

    private void SetSpawnEnemy(bool value)
    {
        EnemyManager.Instance.SpawnEnemy = value;
    }

    private void CheckFPS()
    {
        frames++;
        if (Time.realtimeSinceStartup - lastUpdateTime >= updateInterval)
        {
            fps = frames / (Time.realtimeSinceStartup - lastUpdateTime);
            frameDeltaTime = (Time.realtimeSinceStartup - lastUpdateTime) / frames;
            frames = 0;
            lastUpdateTime = Time.realtimeSinceStartup;
            fpsText.text = $"FPS: {fps:N0} DeltaTime: {frameDeltaTime:F4}";
        }
    }
}
