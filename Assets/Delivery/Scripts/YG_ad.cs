using UnityEngine;
using YG;

public class YG_ad : MonoBehaviour
{
    private void Start()
    {
        YandexGame.FullscreenShow();
    }

    public static void TryShowAdChance(int showChance)
    {
        var random = Random.Range(0, 101);

        if (showChance < random) return;

        YandexGame.FullscreenShow();
    }

    public static bool WillAdShow(int chance)
    {
        var random = Random.Range(0, 101);
        if (chance < random)
            return false;
        else
        {
            Debug.Log("not now");
            return true;
        }
    }
}
