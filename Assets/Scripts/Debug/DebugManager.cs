using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if DEBUG_MODE
public class DebugManager : MonoBehaviour
{
    [SerializeField] private PlayerInputSO inputEvent;
    [SerializeField] private BooleanEventChannelSO needSpawnEnemyEvent;
    [SerializeField] private BooleanEventChannelSO invincibleTextEvent;
    [SerializeField] private BooleanEventChannelSO setInfiniteEnergyEvent;
    [SerializeField] private BooleanEventChannelSO setInfiniteBombEvent;

    [SerializeField] private Canvas debugCanvas;

    private bool isOpenDebugCanvas = false;
    private Player player;

    private void Start()
    {
        inputEvent.eventOnSetDebugMode += InputEvent_onSetDebugMode;
        inputEvent.eventOnNeedSpawnEnemy += InputEvent_onNeedSpawnEnemy;
        inputEvent.eventOnSpawnEnemyNow += InputEvent_onSpawnEnemyNow;
        inputEvent.eventOnSpawnBossNow += InputEvent_onSpawnBossNow;
        inputEvent.eventOnSetPlayerInvincible += InputEvent_eventOnSetPlayerInvincible;
        inputEvent.eventOnSetInfiniteEnergy += InputEvent_eventOnSetInfiniteEnergy;
        inputEvent.eventOnSetInfiniteMissile += InputEvent_eventOnSetInfiniteMissile;
    }

    private void OnDestroy()
    {
        inputEvent.eventOnSetDebugMode -= InputEvent_onSetDebugMode;
        inputEvent.eventOnNeedSpawnEnemy -= InputEvent_onNeedSpawnEnemy;
        inputEvent.eventOnSpawnEnemyNow -= InputEvent_onSpawnEnemyNow;
        inputEvent.eventOnSpawnBossNow -= InputEvent_onSpawnBossNow;
        inputEvent.eventOnSetPlayerInvincible -= InputEvent_eventOnSetPlayerInvincible;
        inputEvent.eventOnSetInfiniteEnergy -= InputEvent_eventOnSetInfiniteEnergy;
        inputEvent.eventOnSetInfiniteMissile += InputEvent_eventOnSetInfiniteMissile;
    }

    private void InputEvent_onSetDebugMode()
    {
        if (!debugCanvas)
        {
            debugCanvas = Instantiate(Resources.Load<Canvas>("Prefabs/Canvas_Debug"), GameObject.FindGameObjectWithTag("MainCanvas").transform);
        }
        if (!player)
        {
            player = FindObjectOfType<Player>();
        }

        debugCanvas.gameObject.SetActive(isOpenDebugCanvas = !isOpenDebugCanvas);
        needSpawnEnemyEvent.RaiseEvent(EnemyManager.Instance.SpawnEnemy);
        invincibleTextEvent.RaiseEvent(player.IsInvincible);
        setInfiniteEnergyEvent.RaiseEvent(player.IsInfiniteEnergy);
        setInfiniteBombEvent.RaiseEvent(player.IsInfiniteBomb);
    }

    private void InputEvent_onNeedSpawnEnemy()
    {
        if (isOpenDebugCanvas)
        {
            needSpawnEnemyEvent.RaiseEvent(EnemyManager.Instance.SpawnEnemy = !EnemyManager.Instance.SpawnEnemy);
        }
    }

    private void InputEvent_onSpawnEnemyNow()
    {
        if (isOpenDebugCanvas)
        {
            EnemyManager.Instance.SpawnEnemyNow();
        }
    }

    private void InputEvent_onSpawnBossNow()
    {
        if (isOpenDebugCanvas)
        {
            EnemyManager.Instance.SpawnBossNow();
        }
    }

    private void InputEvent_eventOnSetPlayerInvincible()
    {
        if (isOpenDebugCanvas)
        {
            invincibleTextEvent.RaiseEvent(player.IsInvincible = !player.IsInvincible);
        }
    }

    private void InputEvent_eventOnSetInfiniteEnergy()
    {
        if (isOpenDebugCanvas)
        {
            setInfiniteEnergyEvent.RaiseEvent(player.IsInfiniteEnergy = !player.IsInfiniteEnergy);
        }
    }

    private void InputEvent_eventOnSetInfiniteMissile()
    {
        if (isOpenDebugCanvas)
        {
            setInfiniteBombEvent.RaiseEvent(player.IsInfiniteBomb = !player.IsInfiniteBomb);
        }
    }
}
#endif