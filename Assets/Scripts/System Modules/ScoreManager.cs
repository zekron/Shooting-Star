using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    #region SCORE DISPLAY

    public int Score => score;

    private int score;
    private int currentScore;

    private Vector3 scoreTextScale = new Vector3(1.2f, 1.2f, 1.2f);

    public void ResetScore()
    {
        score = 0;
        currentScore = 0;
        ScoreDisplay.UpdateText(score);
    }

    public void AddScore(int scorePoint)
    {
        currentScore += scorePoint;
        StartCoroutine(nameof(AddScoreCoroutine));
    }

    private IEnumerator AddScoreCoroutine()
    {
        ScoreDisplay.ScaleText(scoreTextScale);

        while (score < currentScore)
        {
            score += 1;
            ScoreDisplay.UpdateText(score);

            yield return null;
        }

        ScoreDisplay.ScaleText(Vector3.one);
    }

    #endregion

    #region HIGH SCORE SYSTEM

    [Serializable]
    public class PlayerScore : IComparable<PlayerScore>
    {
        public int score;
        public string playerName;

        public PlayerScore(int score, string playerName)
        {
            this.score = score;
            this.playerName = playerName;
        }

        public int CompareTo(PlayerScore playerScore)
        {
            if (score > playerScore.score)
                return 1;
            else if (score == playerScore.score)
                return 0;
            else
                return -1;
        }
    }

    private const int HIGH_SCORE_AMOUNT = 10;

    [Serializable]
    public class PlayerScoreData
    {
        public List<PlayerScore> PlayerScoreList = new List<PlayerScore>(HIGH_SCORE_AMOUNT);
    }

    readonly static string SaveFileName = "player_score.json";
    string playerName = "No Name";

    public bool HasNewHighScore => score > LoadPlayerScoreData().PlayerScoreList[HIGH_SCORE_AMOUNT - 1].score;

    public void SetPlayerName(string newName)
    {
        playerName = newName;
    }

    public void SavePlayerScoreData()
    {
        var playerScoreData = LoadPlayerScoreData();

        playerScoreData.PlayerScoreList.Add(new PlayerScore(score, playerName));
        playerScoreData.PlayerScoreList.Sort((x, y) => y.CompareTo(x));

        SaveSystem.Save(SaveFileName, playerScoreData);
    }

    public PlayerScoreData LoadPlayerScoreData()
    {
        var playerScoreData = new PlayerScoreData();

        if (SaveSystem.SaveFileExists(SaveFileName))
        {
            playerScoreData = SaveSystem.Load<PlayerScoreData>(SaveFileName);
        }
        else
        {
            while (playerScoreData.PlayerScoreList.Count < HIGH_SCORE_AMOUNT)
            {
                playerScoreData.PlayerScoreList.Add(new PlayerScore(0, playerName));
            }

            SaveSystem.Save(SaveFileName, playerScoreData);
        }
        return playerScoreData;
    }

    [UnityEditor.MenuItem("Custom Menu/Save System/Delete Save File")]
    public static void DeletePlayerScoreData()
    {
        SaveSystem.DeleteSaveFile(SaveFileName);
    }
    #endregion
}