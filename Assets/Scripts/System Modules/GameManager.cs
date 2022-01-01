using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    [SerializeField] private GameState gameState = GameState.MainMenu;
    [SerializeField] private GameStateEventChannelSO setGameStateEventSO;

    [SerializeField] private GameObject currentPlayerModel;

    public GameState CurrentGameState
    {
        get => gameState;
        set
        {
            gameState = value;
            setGameStateEventSO.RaiseEvent(value);
        }
    }

    public GameObject CurrentPlayerModel { get => currentPlayerModel; set => currentPlayerModel = value; }

    private void OnEnable()
    {
        Viewport.Initialize();
        OptionManager.LoadPlayerOptionData();
#if !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif

        setGameStateEventSO.OnEventRaised += ReleasePlayer;
    }

    private void ReleasePlayer(GameState state)
    {
        if (state != GameState.Playing) return;

#if UNITY_EDITOR
        if (!GameObject.FindGameObjectWithTag("Player"))
        {
            if (currentPlayerModel)
                ObjectPoolManager.Release(currentPlayerModel);
            else
                throw new System.Exception("Null Player model.");
        }
#else
        ObjectPoolManager.Release(CurrentPlayerModel);
#endif
    }
}