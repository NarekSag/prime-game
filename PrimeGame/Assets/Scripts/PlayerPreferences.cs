using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreferences
{
    public const string HIGHSCORE = "HIGHSCORE";
    public const string CURRENCY = "CURRENCY";

    public static void SetHighScore(int value)
    {
        PlayerPrefs.SetInt(HIGHSCORE, value);
    }

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt(HIGHSCORE);
    }

    public static void SetCurrencyAmount(int value)
    {
        PlayerPrefs.SetInt(CURRENCY, value);
    }

    public static int GetCurrencyAmount()
    {
        return PlayerPrefs.GetInt(CURRENCY);
    }
}
