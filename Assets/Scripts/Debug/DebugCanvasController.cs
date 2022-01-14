#if DEBUG_MODE
using UnityEngine;
using UnityEngine.UI;

public class DebugCanvasController : MonoBehaviour
{
    [SerializeField] private BooleanEventChannelSO needSpawnEnemyEvent;
    [SerializeField] private BooleanEventChannelSO SetInvincibleEvent;
    [SerializeField] private BooleanEventChannelSO setInfiniteEnergyEvent;
    [SerializeField] private BooleanEventChannelSO setInfiniteBombEvent;

    [SerializeField] private Text fpsText;
    [SerializeField] private Text needSpawnEnemyText;
    [SerializeField] private Text invincibleText;
    [SerializeField] private Text infiniteEnergyText;
    [SerializeField] private Text infiniteBombText;

    int frames = 0;
    private float updateInterval = 0.05f;
    private float lastUpdateTime;
    private float fps;
    private float frameDeltaTime;

    #region Unity Functions
    private void OnEnable()
    {
        needSpawnEnemyEvent.OnEventRaised += SetSpawnEnemy;
        SetInvincibleEvent.OnEventRaised += SetPlayerInvincible;
        setInfiniteEnergyEvent.OnEventRaised += SetInfiniteEnergy;
        setInfiniteBombEvent.OnEventRaised += SetInfiniteBomb;
    }

    private void OnDisable()
    {
        needSpawnEnemyEvent.OnEventRaised -= SetSpawnEnemy;
        SetInvincibleEvent.OnEventRaised -= SetPlayerInvincible;
        setInfiniteEnergyEvent.OnEventRaised -= SetInfiniteEnergy;
        setInfiniteBombEvent.OnEventRaised -= SetInfiniteBomb;
    }

    void Start()
    {
    }

    void Update()
    {
        CheckFPS();
    }
    #endregion

    private void SetSpawnEnemy(bool value)
    {
        needSpawnEnemyText.text = string.Format("(F11){0}", value ? "Need Spawn Enemy: ON" : "Need Spawn Enemy: OFF");
        needSpawnEnemyText.color = value ? Color.green : Color.red;
    }

    private void SetPlayerInvincible(bool value)
    {
        invincibleText.text = string.Format("(F1){0}", value ? "Player Invincible: ON" : "Player Invincible: OFF");
        invincibleText.color = value ? Color.green : Color.red;
    }

    private void SetInfiniteEnergy(bool value)
    {
        infiniteEnergyText.text = string.Format("(F2){0}", value ? "Infinite Energy: ON" : "Infinite Energy: OFF");
        infiniteEnergyText.color = value ? Color.green : Color.red;
    }

    private void SetInfiniteBomb(bool value)
    {
        infiniteBombText.text = string.Format("(F3){0}", value ? "Infinite Missile: ON" : "Infinite Missile: OFF");
        infiniteBombText.color = value ? Color.green : Color.red;
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
#endif