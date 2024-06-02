using UnityEngine;
using UnityEngine.UI;
using YG;

public class RewardForAd : MonoBehaviour
{
    [SerializeField] private Delivery_Stats Stats;
    [SerializeField] private Ui_manager Ui_manager;
    [SerializeField] private Delivery_Immersive Immersive;

    // Const to know ID of ads
    private const int engineId = 0;
    private const int speedId = 1;
    private const int capacityId = 2;
    private const int skibidiId = 3;

    // executable buttons for ads
    [SerializeField] private Button engineRewardButton;
    [SerializeField] private Button speedRewardButton;
    [SerializeField] private Button capacityRewardButton;
    [SerializeField] private Button skibidiRewardButton;


    private void Start()
    {
        // I dont understand clearly srry(mostly why in start) :(
        engineRewardButton.onClick.AddListener(delegate { OpenSkibidiAd(engineId); });
        speedRewardButton.onClick.AddListener(delegate { OpenSkibidiAd(speedId); });
        capacityRewardButton.onClick.AddListener(delegate { OpenSkibidiAd(capacityId); });

        skibidiRewardButton.onClick.AddListener(delegate { OpenSkibidiAd(skibidiId); });
    }
    private void OnEnable()
    {
        // Method returns id that used in void Rewarded uphere
        YandexGame.RewardVideoEvent += Rewarded;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Rewarded;
    }

    private void OpenSkibidiAd(int id)
    {
        // Call ad method 
        YandexGame.RewVideoShow(id);
    }

    void Rewarded(int id)
    {
        switch (id)
        {
            // Upgrage for ad
            case engineId or speedId or capacityId:
                Stats.UpgradeTierOf(id);
                Ui_manager.SetNewMaxTier(id);
                Ui_manager.CheckTiers(id);
                Ui_manager.DisableRewAdButton(id);
                break;

            // Money or skibidi for ad
            case skibidiId:
                int chance = Random.Range(0, 100);
                int threshold = 40;
                if (chance >= threshold)
                    YG_Saves.SaveCoins(YG_Saves.LoadCoins() + GetManyCoins());
                else
                    YG_Saves.SaveCoins(YG_Saves.LoadCoins() + GetLessCoins());

                Ui_manager.ReloadPlayerCoins();
                break;
        }
    }

    private int GetLessCoins()
    {
        Immersive.CollectCoins();
        return Random.Range(10, 14);
    }

    private int GetManyCoins()
    {
        Immersive.CollectCoins();
        return Random.Range(15, 25);
    }
}
