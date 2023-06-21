using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Prefs
{
    public static int bestScore
    {
        get => PlayerPrefs.GetInt(GameConst.BEST_SCORE, 0);

        set
        {
            int curScore = PlayerPrefs.GetInt(GameConst.BEST_SCORE);

            if (value > curScore)
            {
                PlayerPrefs.SetInt(GameConst.BEST_SCORE, value);
            }
        }
    }
}
