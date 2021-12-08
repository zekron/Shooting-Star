using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringUIController : MonoBehaviour
{
    [Header("==== BACKGROUND ====")]
    [SerializeField] private Image background;
    [SerializeField] private Sprite[] backgroundSprites;

    [Header("==== SCORING SCREEN ====")]
    [SerializeField] private Canvas scoringScreenCanvas;
    [SerializeField] private Text playerScoreText;
    [SerializeField] private Button buttonMainMenu;
    [SerializeField] private Transform highScoreLeaderboardContainer;

    [Header("==== HIGH SCORE SCREEN ====")]
    [SerializeField] private Canvas newHighScoreScreenCanvas;
    [SerializeField] private Button buttonCancel;
    [SerializeField] private Button buttonSubmit;
    [SerializeField] private InputField playerNameInputField;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        ShowRandomBackground();

        if (ScoreManager.Instance.HasNewHighScore)
        {
            ShowNewHighScoreScreen();
        }
        else
        {
            ShowScoringScreen();
        }

        ButtonPressedBehavior.buttonFunctionTable.Add(buttonMainMenu.gameObject.GetInstanceID(), OnButtonMainMenuClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonSubmit.gameObject.GetInstanceID(), OnButtonSubmitClicked);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonCancel.gameObject.GetInstanceID(), HideNewHighScoreScreen);

        GameManager.CurrentGameState = GameState.Scoring;
    }

    void OnDisable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    void ShowRandomBackground()
    {
        background.sprite = backgroundSprites[Random.Range(0, backgroundSprites.Length)];
    }

    void ShowNewHighScoreScreen()
    {
        newHighScoreScreenCanvas.enabled = true;
        UIInput.Instance.SelectUI(buttonCancel);
    }

    void HideNewHighScoreScreen()
    {
        newHighScoreScreenCanvas.enabled = false;
        ScoreManager.Instance.SavePlayerScoreData();
        ShowRandomBackground();
        ShowScoringScreen();
    }

    void ShowScoringScreen()
    {
        scoringScreenCanvas.enabled = true;
        playerScoreText.text = ScoreManager.Instance.Score.ToString();
        UIInput.Instance.SelectUI(buttonMainMenu);
        UpdateHighScoreLeaderboard();
    }

    void UpdateHighScoreLeaderboard()
    {
        var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData().PlayerScoreList;

        for (int i = 0; i < highScoreLeaderboardContainer.childCount; i++)
        {
            var child = highScoreLeaderboardContainer.GetChild(i);

            child.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();
            child.Find("Score").GetComponent<Text>().text = playerScoreList[i].score.ToString();
            child.Find("Name").GetComponent<Text>().text = playerScoreList[i].playerName;
        }
    }

    void OnButtonMainMenuClicked()
    {
        scoringScreenCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }

    void OnButtonSubmitClicked()
    {
        if (!string.IsNullOrEmpty(playerNameInputField.text))
        {
            ScoreManager.Instance.SetPlayerName(playerNameInputField.text);
        }

        HideNewHighScoreScreen();
    }
}