using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    public static System.Action onGameOver;

    public static GameState CurrentGameState { get => Instance.gameState; set => Instance.gameState = value; }

    [SerializeField] private GameState gameState = GameState.Playing;
    public GameObject CurrentPlayerModel;

    private void OnEnable()
    {
        Viewport.Initialize();
        OptionManager.LoadPlayerOptionData();
#if !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif
    }

    private void SetCurrentPlayerModel(GameObject gameObject)
    {
        CurrentPlayerModel = gameObject;
    }
}

public enum GameState
{
    Playing,
    Paused,
    GameOver,
    Scoring
}