using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;
using TMPro;
using DG.Tweening;
using YG;

public class Ui_manager : MonoBehaviour
{
    [SerializeField] private int adAppearChance;
    [SerializeField] private float adTimerMultiplier;
    [Space(15)]

    // Camera switcher
    [SerializeField] private CinemachineFreeLook[] virtualCams;

    [SerializeField] private Delivery_Stats Stats;

    // Canvas parents
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject upgradeMenu;
    private bool isUgradeEnabled;

    [SerializeField] private GameObject leaderboard;

    // Pop-up windows
    [SerializeField] private GameObject[] upgradeWindows;

    // To enable/disable arrow-buttons
    [SerializeField] private GameObject[] upgradeButtons;
    [SerializeField] private GameObject[] downgradeButtons;

    // Buy-tier buttons
    [SerializeField] private Button[] buyButtons;

    // Buy-tier price text
    [SerializeField] private TMP_Text[] tierPriceTexts;

    // Buy buttons Vectors
    private List<Vector3> buyButtonsPos = new();

    // For checking status (count) of tier
    [SerializeField] private string[] tierNames;
    private List<int> tiers = new();

    // Max tier
    private const int _maxTier = 10;

    // Text and slider in pop-ups
    [SerializeField] private TMP_Text[] tierTexts;
    [SerializeField] private Slider[] tierSliders;

    // Rewarded ad button
    [SerializeField] private GameObject skibidiLogo;
    [SerializeField] private GameObject skibidiText;
    [SerializeField] private GameObject coinLogo;
    [SerializeField] private GameObject coinText;
    [SerializeField] private GameObject _lock;

    // Executable buttons for ads
    [SerializeField] private GameObject[] tierAdRewardButtons;

    // Bait timer text
    [SerializeField] private TMP_Text[] baitTimerTexts;

    // Is need to start bait timer
    [SerializeField] private bool[] isNeedTimers;

    // Bait timer and maxValue float for ad buttons
    [SerializeField] private float[] AdTimerFloats;
    private const float _maxTimerValue = 5f;

    // Show all the money player have
    [SerializeField] private TMP_Text playerCoinsText;

    // Skibidi toggle
    [SerializeField] private Toggle skibidiToggle;

    // Music
    private MusicManager MusicManager;
    [SerializeField] private Button musicOnButton;
    [SerializeField] private Button musicOffButton;

    private void Awake()
    {
        MusicManager = GameObject.FindWithTag("MusicPlayer").GetComponent<MusicManager>();
    }

    private void Start()
    {
        // Waiting for button press, then play the script
        buyButtons[0].onClick.AddListener(delegate { BuyTier(0, YG_Saves.LoadEngine()); });
        buyButtons[1].onClick.AddListener(delegate { BuyTier(1, YG_Saves.LoadSpeed()); });
        buyButtons[2].onClick.AddListener(delegate { BuyTier(2, YG_Saves.LoadCapacity()); });

        // Skibidi toggle
        skibidiToggle.onValueChanged.AddListener(delegate { Stats.ToggleSkibidi(); });

        // Music buttons
        musicOnButton.onClick.AddListener(delegate { MusicSwitch(0); });
        musicOffButton.onClick.AddListener(delegate { MusicSwitch(1); });

        if (PlayerPrefs.GetInt("transFromMainScene", 0) == 1)
        {
            Debug.Log("hiInterface");
            PlayerPrefs.SetInt("transFromMainScene", 0);
            GetData();
        }
    }

    #region Yandex
    // Подписываемся на событие GetDataEvent в OnEnable
    private void OnEnable() => YandexGame.GetDataEvent += GetData;

    // Отписываемся от события GetDataEvent в OnDisable
    private void OnDisable() => YandexGame.GetDataEvent -= GetData;

    public void GetData()
    {
        // Получаем данные из плагина и делаем с ними что хотим

        for (int i = 0; i < 3; i++)
        {
            buyButtonsPos.Add(buyButtons[i].gameObject.transform.position);
        }

        for (int i = 0; i < 3; i++)
        {
            // Adding int values to list from PPrefs
            tiers.Add(PlayerPrefs.GetInt(tierNames[i], 0));

            // Update all valuews in pop-up to actual
            CheckTiers(i);
        }

        ReloadPlayerCoins();

        SkibidiUiControl();

        // Music
        MusicManager = GameObject.FindWithTag("MusicPlayer").GetComponent<MusicManager>();
        if (PlayerPrefs.GetInt("music", 1) == 0)
        {
            MusicSwitch(0);
        }
        else
        {
            MusicSwitch(1);
        }
    }
    #endregion

    private void Update()
    {
        // Upgrade-tier-for-ad button timers
        for(int i = 0; i < 3; i++)
            AdDisableTimer(i, ref isNeedTimers[i], ref AdTimerFloats[i]);
    }

    // Cam switcher; not only for buttons
    #region Camera priority
    public void ToCamera_0()
    {
        SwitchCamera(0);
    }
    public void ToCamera_1()
    {
        SwitchCamera(1);
    }
    public void ToCamera_2()
    {
        SwitchCamera(2);
    }
    public void ToCamera_3()
    {
        SwitchCamera(3);
    }
    private void SwitchCamera(int camIndex)
    {
        virtualCams[camIndex].m_Priority = 11;

        foreach (CinemachineFreeLook cam in virtualCams )
        {
            if (cam != virtualCams[camIndex])
            {
                cam.m_Priority = 10;
            }
        }
    }
    #endregion

    #region Upgrade UI
    // For buttons
    public void UpgradeMenu_enable_switch()
    {
        if (!isUgradeEnabled)
        {
            isUgradeEnabled = true;

            mainMenu.SetActive(false);
            upgradeMenu.SetActive(true);

            leaderboard.SetActive(false);

            ToCamera_1();
        }
        else
        {
            isUgradeEnabled = false;

            mainMenu.SetActive(true);
            upgradeMenu.SetActive(false);

            leaderboard.SetActive(true);

            ToCamera_0();

            DisableUpgrade();
        }
    }

    // Show pop-ups buttons
    public void EnableUpgrade_0()
    {
        SwitchUpgrade(0);
        ToCamera_1();
    }
    public void EnableUpgrade_1()
    {
        SwitchUpgrade(1);
        ToCamera_2();
    }
    public void EnableUpgrade_2()
    {
        SwitchUpgrade(2); 
        ToCamera_3();

    }

    // Close pop-up if enabling another
    private void SwitchUpgrade(int windowIndex)
    {
        upgradeWindows[windowIndex].SetActive(true);

        foreach (GameObject window in upgradeWindows)
        {
            if (window != upgradeWindows[windowIndex])
            {
                window.SetActive(false);
            }
        }
    }

    // If closing Upgrade UI - close pop-ups
    public void DisableUpgrade()
    {
        foreach (GameObject window in upgradeWindows)
        {
            if (window.activeSelf == true)
            {
                window.SetActive(false);
            }
        }
    }

    // Upgrades; for buttons
    public void UpgradeEngine()
    {
        Stats.UpgradeTierOf(0);
        CheckTiers(0);
    }
    public void UpgradeSpeed()
    {
        Stats.UpgradeTierOf(1); 
        CheckTiers(1);
    }
    public void UpgradeCapacity()
    {
        Stats.UpgradeTierOf(2);
        CheckTiers(2);
    }
    // Downgrades; for buttons
    public void DowngradeEngine()
    {
        Stats.DowngradeTierOf(0);
        CheckTiers(0);
        DisableRewAdButton(0);
    }
    public void DowngradeSpeed()
    {
        Stats.DowngradeTierOf(1);
        CheckTiers(1);
        DisableRewAdButton(1);
    }
    public void DowngradeCapacity()
    {
        Stats.DowngradeTierOf(2);
        CheckTiers(2);
        DisableRewAdButton(2);
    }

    // Stat indexes: 1) Engine 2) Speed 3) Capacity
    public void CheckTiers(int statIndex)
    {
        // After "upgrade" load actual tier int from prefs
        switch (statIndex)
        {
            case 0:
                tiers[statIndex] = YG_Saves.LoadEngine(); break;
            case 1:
                tiers[statIndex] = YG_Saves.LoadSpeed(); break;
            case 2:
                tiers[statIndex] = YG_Saves.LoadCapacity(); break;

        }

        // Hide down/up arrow-buttons if tier is 0 or 10 accordingly 
        if (tiers[statIndex] >= _maxTier)
        {
            upgradeButtons[statIndex].SetActive(false);
        }
        else
        {
            upgradeButtons[statIndex].SetActive(true);
        }

        if (tiers[statIndex] <= 0)
        {
            downgradeButtons[statIndex].SetActive(false);
        }
        else
        {
            downgradeButtons[statIndex].SetActive(true);
        }

        // Sync slider value with tier value 
        tierSliders[statIndex].value = tiers[statIndex];

        // Change text in pop-up to the actual
        tierTexts[statIndex].text = tiers[statIndex].ToString();

        // As it saying
        SwitchBuyButton(statIndex);
    }


    // Only for ad way of upgrade
    public void SetNewMaxTier(int statIndex)
    {
        switch (statIndex)
        {
            case 0:
                // Was PlayerPrefs.SetInt("engineMaxTier", PlayerPrefs.GetInt("engineTier", 0));
                YG_Saves.SaveMaxEngine(YG_Saves.LoadEngine());
                break;
            case 1:
                YG_Saves.SaveMaxSpeed(YG_Saves.LoadSpeed());
                break;
            case 2:
                YG_Saves.SaveMaxCapacity(YG_Saves.LoadCapacity());
                break;
        }
    }
    #endregion

    #region Upgrade via buying
    private void SwitchBuyButton(int statIndex)
    {
        if (upgradeButtons[statIndex].activeSelf == false)
        {
            buyButtons[statIndex].gameObject.SetActive(false);
            DisableRewAdButton(statIndex);
            return;
        }

        switch (statIndex)
        {
            case 0:
                if (YG_Saves.LoadEngine() == YG_Saves.LoadMaxEngine()) 
                {
                    buyButtons[statIndex].gameObject.SetActive(true);
                    upgradeButtons[statIndex].gameObject.SetActive(false);
                    SetPrice(statIndex, YG_Saves.LoadEngine());
                }
                else buyButtons[statIndex].gameObject.SetActive(false);
                break;

            case 1:
                if (YG_Saves.LoadSpeed() == YG_Saves.LoadMaxSpeed())
                {
                    buyButtons[statIndex].gameObject.SetActive(true);
                    upgradeButtons[statIndex].gameObject.SetActive(false);
                    SetPrice(statIndex, YG_Saves.LoadSpeed());
                }
                else buyButtons[statIndex].gameObject.SetActive(false);
                break;

            case 2:
                if (YG_Saves.LoadCapacity() == YG_Saves.LoadMaxCapacity())
                {
                    buyButtons[statIndex].gameObject.SetActive(true);
                    upgradeButtons[statIndex].gameObject.SetActive(false);
                    SetPrice(statIndex, YG_Saves.LoadCapacity());
                }
                else buyButtons[statIndex].gameObject.SetActive(false);
                break;
        }

        if (buyButtons[statIndex].gameObject.activeSelf == false) 
            return;

        // Chance for ad to appear
        AdUpgradeChance(adAppearChance, statIndex);

    }

    private void SetPrice(int statIndex, int currTier)
    {
        tierPriceTexts[statIndex].text = Stats.priceArray[currTier + 1].ToString();
    }
    
    // Called from start by onClick
    private void BuyTier(int statIndex, int currTier)
    {
        switch (statIndex)
        {
            case 0:
                if (YG_Saves.LoadCoins() < Stats.priceArray[currTier + 1])
                {
                    // UI message
                    NotEnoughMoney(statIndex);
                    break;
                }
                YG_Saves.SaveCoins(YG_Saves.LoadCoins() - Stats.priceArray[YG_Saves.LoadMaxEngine() + 1]);
                YG_Saves.SaveMaxEngine(YG_Saves.LoadMaxEngine() + 1);
                UpgradeEngine();
                ReloadPlayerCoins();
                break;

            case 1:
                if (YG_Saves.LoadCoins() < Stats.priceArray[currTier + 1])
                {
                    NotEnoughMoney(statIndex);
                    break;
                }
                YG_Saves.SaveCoins(YG_Saves.LoadCoins() - Stats.priceArray[YG_Saves.LoadMaxSpeed() + 1]);
                YG_Saves.SaveMaxSpeed(YG_Saves.LoadMaxSpeed() + 1);
                UpgradeSpeed();
                ReloadPlayerCoins();
                break;

            case 2:
                if (YG_Saves.LoadCoins() < Stats.priceArray[currTier + 1])
                {
                    NotEnoughMoney(statIndex);
                    break;
                }
                YG_Saves.SaveCoins(YG_Saves.LoadCoins() - Stats.priceArray[YG_Saves.LoadMaxCapacity() + 1]);
                YG_Saves.SaveMaxCapacity(YG_Saves.LoadMaxCapacity() + 1);
                UpgradeCapacity();
                ReloadPlayerCoins();
                break;
        }
    }

    private void NotEnoughMoney(int statIndex)
    {
        buyButtons[statIndex].gameObject.transform.DOShakePosition(2.0f, strength: new Vector3(10, 0, 0), vibrato: 5, randomness: 0, snapping: true, fadeOut: true).OnComplete(() => {
        buyButtons[statIndex].gameObject.transform.DOMove(buyButtonsPos[statIndex], .2f);});
    }
    #endregion

    #region Upgrade via adv

    // After enabling buy button enable upgrade-for-ad with chance
    private void AdUpgradeChance(int chance, int buttonIndex)
    {
        if (buyButtons[buttonIndex].gameObject.activeSelf == false) return;

        // If tier exceed or equals __maxTier(10) leave the void
            if (tiers[buttonIndex] >= _maxTier)
            return;

        // if chance is not more than random value leave void
        var random = Random.Range(0, 101);
        if (chance < random)
            return;

        isNeedTimers[buttonIndex] = true;
        AdTimerFloats[buttonIndex] = _maxTimerValue;
    }

    public void DisableRewAdButton(int buttonIndex)
    {
        tierAdRewardButtons[buttonIndex].SetActive(false);
        isNeedTimers[buttonIndex] = false;
    }

    // Ad timers
    private void AdDisableTimer(int buttonIndex, ref bool isNeedTimer, ref float timer)
    {
        if (isNeedTimer)
        {
            tierAdRewardButtons[buttonIndex].SetActive(true);

            if (timer > 0f)
            {
                timer -= Time.deltaTime * adTimerMultiplier;
                baitTimerTexts[buttonIndex].text = Mathf.Round(timer).ToString();
            }
            else
            {
                tierAdRewardButtons[buttonIndex].SetActive(false);
                isNeedTimer = false;
            }
        }
    }
    #endregion
    
    #region Skibidi UI control
    public void SkibidiUiControl()
    {
        if (YG_Saves.LoadIsGotSkibidi() == true)
        {
            skibidiLogo.SetActive(false);
            skibidiText.SetActive(false);

            coinLogo.SetActive(true);
            coinText.SetActive(true);

            _lock.SetActive(false);
        }
        else
        {
            skibidiLogo.SetActive(true);
            skibidiText.SetActive(true);

            coinLogo.SetActive(false);
            coinText.SetActive(false);

            _lock.SetActive(true);
        }

        if (YG_Saves.LoadIsEnableSkibidi() == true)
        {
            skibidiToggle.isOn = true;
            Stats.EnableSkibidi();
        }
        else
        {
            skibidiToggle.isOn = false;
            Stats.DisableSkibidi();
        }
    }
    #endregion

    public void ReloadPlayerCoins()
    {
        playerCoinsText.text = YG_Saves.LoadCoins().ToString();
    }

    private void MusicSwitch(int onOff)
    {
        if (onOff == 0)
        {
            MusicManager.PauseMusic();
            PlayerPrefs.SetInt("music", 0);
            musicOnButton.gameObject.SetActive(false);
            musicOffButton.gameObject.SetActive(true);
        }
        else
        {
            MusicManager.PlayMusic();
            PlayerPrefs.SetInt("music", 1);
            musicOnButton.gameObject.SetActive(true);
            musicOffButton.gameObject.SetActive(false);
        }
    }

    public void ToCityScene()
    {
        SceneManager.LoadScene(1);
    }
}
