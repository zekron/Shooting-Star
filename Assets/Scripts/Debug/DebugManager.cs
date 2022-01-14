using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if DEBUG_MODE
public class DebugManager : MonoBehaviour
{
    [SerializeField] private PlayerInputSO inputEvent;
    [SerializeField] private BooleanEventChannelSO needSpawnEnemyEvent;
    [SerializeField] private VoidEventChannelSO spawnEnemyNowEvent;
    [SerializeField] private VoidEventChannelSO spawnBossNowEvent;
    [SerializeField] private BooleanEventChannelSO setinvincibleEvent;
    [SerializeField] private BooleanEventChannelSO setInfiniteEnergyEvent;
    [SerializeField] private BooleanEventChannelSO setInfiniteBombEvent;

    [SerializeField] private Canvas debugCanvas;

    private bool isOpenDebugCanvas = false;

    private bool playerIsInvincible = false;
    private bool playerIsInfiniteEnergy = false;
    private bool playerIsInfiniteBomb = false;
    private bool enemyIsSpawnEnemyAuto = true;

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

        debugCanvas.gameObject.SetActive(isOpenDebugCanvas = !isOpenDebugCanvas);
        needSpawnEnemyEvent.RaiseEvent(enemyIsSpawnEnemyAuto);
        setinvincibleEvent.RaiseEvent(playerIsInvincible);
        setInfiniteEnergyEvent.RaiseEvent(playerIsInfiniteEnergy);
        setInfiniteBombEvent.RaiseEvent(playerIsInfiniteBomb);
    }

    private void InputEvent_onNeedSpawnEnemy()
    {
        if (isOpenDebugCanvas)
        {
            needSpawnEnemyEvent.RaiseEvent(enemyIsSpawnEnemyAuto = !enemyIsSpawnEnemyAuto);
        }
    }

    private void InputEvent_onSpawnEnemyNow()
    {
        if (isOpenDebugCanvas)
        {
            spawnEnemyNowEvent.RaiseEvent();
        }
    }

    private void InputEvent_onSpawnBossNow()
    {
        if (isOpenDebugCanvas)
        {
            spawnBossNowEvent.RaiseEvent();
        }
    }

    private void InputEvent_eventOnSetPlayerInvincible()
    {
        if (isOpenDebugCanvas)
        {
            setinvincibleEvent.RaiseEvent(playerIsInvincible = !playerIsInvincible);
        }
    }

    private void InputEvent_eventOnSetInfiniteEnergy()
    {
        if (isOpenDebugCanvas)
        {
            setInfiniteEnergyEvent.RaiseEvent(playerIsInfiniteEnergy = !playerIsInfiniteEnergy);
        }
    }

    private void InputEvent_eventOnSetInfiniteMissile()
    {
        if (isOpenDebugCanvas)
        {
            setInfiniteBombEvent.RaiseEvent(playerIsInfiniteBomb = !playerIsInfiniteBomb);
        }
    }
}
#endif