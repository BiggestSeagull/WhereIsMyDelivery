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
                int skibidiAdCount = YG_Saves.LoadSkibidiAdCount();
                switch (skibidiAdCount)
                {
                    case 0:
                        YG_Saves.SaveSkibidiAdCount(skibidiAdCount + 1);

                        YG_Saves.SaveCoins(YG_Saves.LoadCoins() + GetSkibidiChance(10));
                        Ui_manager.ReloadPlayerCoins();
                        break;
                    case 1:
                        YG_Saves.SaveSkibidiAdCount(skibidiAdCount + 1);

                        YG_Saves.SaveCoins(YG_Saves.LoadCoins() + GetSkibidiChance(40));
                        Ui_manager.ReloadPlayerCoins();
                        break;
                    case 2:
                        YG_Saves.SaveSkibidiAdCount(skibidiAdCount + 1);

                        GetSkibidi();
                        break;
                    case 3:
                        YG_Saves.SaveCoins(YG_Saves.LoadCoins() + GetManyCoins());
                        Ui_manager.ReloadPlayerCoins();
                        break;
                }
                break;
        }
    }

    // Have not got skibidi, checking luck
    private int GetSkibidiChance(int baseChance)
    {
        var randomChance = Random.Range(0, 101);

        if (baseChance <= randomChance)
        {
            // Unluck
            Immersive.CollectCoins();
            return Random.Range(10, 20);
        }
        else
        {
            // You are lucky
            GetSkibidi();
            return 0;
        }
    }
    private void GetSkibidi()
    {
        Debug.Log("getSkibi");
        YG_Saves.SaveIsGotSkibidi(true);
        YG_Saves.SaveSkibidiAdCount(3);

        Ui_manager.SkibidiUiControl();
    }
    private int GetManyCoins()
    {
        Immersive.CollectCoins();
        return Random.Range(15, 25);
    }
}
