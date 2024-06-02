using UnityEngine;
using YG;

public class YG_Saves : MonoBehaviour
{
    private static YG_Saves instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static int LoadCoins()
    {
        return YandexGame.savesData.coins;
    }

    public static int LoadEngine()
    {
        return YandexGame.savesData.engine;
    }
    public static int LoadSpeed()
    {
        return YandexGame.savesData.speed;
    }
    public static int LoadCapacity()
    {
        return YandexGame.savesData.capacity;
    }

    // Max tier that player got
    public static int LoadMaxEngine()
    {
        return YandexGame.savesData.maxEngine;
    }
    public static int LoadMaxSpeed()
    {
        return YandexGame.savesData.maxSpeed;
    }
    public static int LoadMaxCapacity()
    {
        return YandexGame.savesData.maxCapacity;
    }

    public static int LoadLbScore()
    {
        return YandexGame.savesData.leaderboardScore;
    }

    public static void SaveCoins(int valueToSave)
    {
        YandexGame.savesData.coins = valueToSave;
        SaveProgress();
    }

    public static void SaveEngine(int valueToSave)
    {
        YandexGame.savesData.engine = valueToSave;
        SaveProgress();
    }
    public static void SaveSpeed(int valueToSave)
    {
        YandexGame.savesData.speed = valueToSave;
        SaveProgress();
    }
    public static void SaveCapacity(int valueToSave)
    {
        YandexGame.savesData.capacity = valueToSave;
        SaveProgress();
    }

    public static void SaveLbScore(int valueToSave)
    {
        YandexGame.savesData.leaderboardScore = valueToSave;
        SaveProgress();
    }

    // Max tier that player got
    public static void SaveMaxEngine(int val)
    {
        YandexGame.savesData.maxEngine = val;
        SaveProgress();
    }
    public static void SaveMaxSpeed(int val)
    {
        YandexGame.savesData.maxSpeed = val;
        SaveProgress();
    }
    public static void SaveMaxCapacity(int val)
    {
        YandexGame.savesData.maxCapacity = val;
        SaveProgress();
    }

    public static void SaveProgress()
    {
        YandexGame.SaveProgress();
    }
}
