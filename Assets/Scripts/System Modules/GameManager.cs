using System;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField] private GameState gameState = GameState.MainMenu;
    [SerializeField] private IntEventChannelSO profileIndexEventSO;
    [SerializeField] private GameStateEventChannelSO setGameStateEventSO;

    [SerializeField] private GameObject[] playerModels;
#if UNITY_EDITOR
    [SerializeField]
#endif
    private int playerModelIndex;

    public GameState CurrentGameState
    {
        get => gameState;
        set
        {
            gameState = value;
            setGameStateEventSO.RaiseEvent(value);
        }
    }

    private void OnEnable()
    {
        Viewport.Initialize();
        OptionManager.LoadPlayerOptionData();
#if !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif

        setGameStateEventSO.OnEventRaised += ReleasePlayer;
        profileIndexEventSO.OnEventRaised += RefreshPlayerModelIndex;
    }

    private void RefreshPlayerModelIndex(int value)
    {
        playerModelIndex = value;
    }

    private void ReleasePlayer(GameState state)
    {
        if (state != GameState.Playing) return;

#if UNITY_EDITOR
        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            ObjectPoolManager.Release(playerModels[playerModelIndex]);
        }
#else
        ObjectPoolManager.Release(playerModels[playerModelIndex]);
#endif
    }
}