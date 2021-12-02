using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    #region SCORE DISPLAY
    [SerializeField] private IntEventChannelSO updateTotalScoreEventSO;

    public int Score => totalScore;

    /// <summary>
    /// 当前总得分
    /// </summary>
    private int totalScore;

    protected override void Awake()
    {
        base.Awake();

        updateTotalScoreEventSO.OnEventRaised += AddTotalScore;
    }

    public void ResetScore()
    {
        totalScore = 0;
        updateTotalScoreEventSO.RaiseEvent(totalScore);
    }

    private void AddTotalScore(int scorePoint)
    {
        totalScore += scorePoint;
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

    public bool HasNewHighScore => totalScore > LoadPlayerScoreData().PlayerScoreList[HIGH_SCORE_AMOUNT - 1].score;

    public void SetPlayerName(string newName)
    {
        playerName = newName;
    }

    public void SavePlayerScoreData()
    {
        var playerScoreData = LoadPlayerScoreData();

        playerScoreData.PlayerScoreList.Add(new PlayerScore(totalScore, playerName));
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

#if UNITY_EDITOR

    [UnityEditor.MenuItem("Custom Menu/Save System/Delete Save File")]
    public static void DeletePlayerScoreData()
    {
        SaveSystem.DeleteSaveFile(SaveFileName);
    }
#endif
    #endregion
}