using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager
{
    private readonly static string SaveFileName = "player_option.json";

    private static PlayerOptionData optionData;

    public static PlayerOptionData OptionData { get => optionData; }

    public static void SavePlayerOptionData(float sfx, float bgm, bool needShow)
    {
        optionData = new PlayerOptionData(sfx, bgm, needShow);
        SaveSystem.Save(SaveFileName, optionData);
    }
    public static void SavePlayerOptionData(PlayerOptionData data)
    {
        SaveSystem.Save(SaveFileName, data);
    }

    public static PlayerOptionData LoadPlayerOptionData()
    {
        if (SaveSystem.SaveFileExists(SaveFileName))
        {
            optionData = SaveSystem.Load<PlayerOptionData>(SaveFileName);
        }
        else
        {
            optionData = new PlayerOptionData(1, 1, false);

            SaveSystem.Save(SaveFileName, optionData);
        }
        return optionData;
    }
}
[Serializable]
public class PlayerOptionData
{
    public float SFXVolume;
    public float BGMVolume;
    public bool NeedShowHealthBar;

    public PlayerOptionData(float sfx, float bgm, bool needShow)
    {
        SFXVolume = sfx;
        BGMVolume = bgm;
        NeedShowHealthBar = needShow;
    }
    public PlayerOptionData() { }
}
