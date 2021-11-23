using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    public static System.Action onGameOver;

    public static GameState CurrentGameState { get => Instance.gameState; set => Instance.gameState = value; }

    [SerializeField] private GameState gameState = GameState.Playing;

    private void OnEnable()
    {
        Viewport.Initialize();
    }
}

public enum GameState
{
    Playing,
    Paused,
    GameOver,
    Scoring
}