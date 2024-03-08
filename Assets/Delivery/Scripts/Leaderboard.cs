using UnityEngine;
using YG;

public class Leaderboard : MonoBehaviour
{
    public static void NewWrite()
    {
        // Prefs are OK here, thrust yourself xd
        int _newRecord = YG_Saves.LoadLbScore() + PlayerPrefs.GetInt("lbVarToSave", 0);
        YG_Saves.SaveLbScore(_newRecord);

        YandexGame.NewLeaderboardScores("delivered", _newRecord);
    }
}
