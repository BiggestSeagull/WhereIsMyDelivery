using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using YG;

public class Delivery_Shop : MonoBehaviour
{
    // Links to other scripts
    private Delivery_OrderManager OrderManager;
    private Delivery_Stats Stats;

    // List of orders
    public List<OrderObject> thisShopOrders = new();
    
    private OrderDisplay displayScript_1;
    private OrderDisplay displayScript_2;
    private OrderDisplay displayScript_3;

    // Shop UI
    private GameObject ShopUI;

    private GameObject enterShopText;
    private GameObject enterShopButton;
    private GameObject bgExitUI;
    private GameObject shopWindowUI;

    private GameObject slot_1_UI;
    private GameObject slot_2_UI;
    private GameObject slot_3_UI;

    private GameObject acceptButton_1;
    private GameObject acceptButton_2;
    private GameObject acceptButton_3;

    private GameObject lockOrder_UI_1;
    private GameObject lockOrder_UI_2;
    private GameObject lockOrder_UI_3;

    private OrderObject orderDisplay_1;
    private OrderObject orderDisplay_2;
    private OrderObject orderDisplay_3;

    private TMP_Text lockTimerText_1;
    private TMP_Text lockTimerText_2;
    private TMP_Text lockTimerText_3;

    private float locktime = 10f;
    private float f_lockTimer_1;
    private float f_lockTimer_2;
    private float f_lockTimer_3;

    // Is in shop trigger
    private bool isInShop;

    // Is counted down
    private bool isCounted_1 = true;
    private bool isCounted_2 = true;
    private bool isCounted_3 = true;

    // Enter shop delay
    private float shopDelay;

    // Yandex
    private string language;

    void Start()
    {
        // Private names define cuz shops are prefabs
        #region
        OrderManager = GameObject.FindWithTag("GameManager").GetComponent<Delivery_OrderManager>();
        Stats = GameObject.FindWithTag("Player").GetComponent<Delivery_Stats>();

        ShopUI = GameObject.FindWithTag("Delivery_ShopUI");

        enterShopButton = ShopUI.transform.GetChild(0).gameObject;
        enterShopText = ShopUI.transform.GetChild(1).gameObject;
        bgExitUI = ShopUI.transform.GetChild(2).gameObject;
        shopWindowUI = ShopUI.transform.GetChild(3).gameObject;

        slot_1_UI = shopWindowUI.transform.GetChild(0).gameObject;
        slot_2_UI = shopWindowUI.transform.GetChild(1).gameObject;
        slot_3_UI = shopWindowUI.transform.GetChild(2).gameObject;

        acceptButton_1 = slot_1_UI.transform.GetChild(0).gameObject;
        acceptButton_2 = slot_2_UI.transform.GetChild(0).gameObject;
        acceptButton_3 = slot_3_UI.transform.GetChild(0).gameObject;

        lockOrder_UI_1 = slot_1_UI.transform.GetChild(1).gameObject;
        lockOrder_UI_2 = slot_2_UI.transform.GetChild(1).gameObject;
        lockOrder_UI_3 = slot_3_UI.transform.GetChild(1).gameObject;

        displayScript_1 = slot_1_UI.transform.GetChild(2).gameObject.GetComponent<OrderDisplay>();
        displayScript_2 = slot_2_UI.transform.GetChild(2).gameObject.GetComponent<OrderDisplay>();
        displayScript_3 = slot_3_UI.transform.GetChild(2).gameObject.GetComponent<OrderDisplay>();

        orderDisplay_1 = displayScript_1.orderObject;
        orderDisplay_2 = displayScript_2.orderObject;
        orderDisplay_3 = displayScript_3.orderObject;

        lockTimerText_1 = lockOrder_UI_1.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        lockTimerText_2 = lockOrder_UI_2.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        lockTimerText_3 = lockOrder_UI_3.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
        #endregion

        // Yandex
        if (SceneManager.GetActiveScene().buildIndex != 0 | PlayerPrefs.GetInt("transFromMainScene", 0) == 1)
        {
            PlayerPrefs.SetInt("transFromMainScene", 0);
            GetData();
        }

        // Mobile enter/exit shop buttons
        enterShopButton.GetComponent<Button>().onClick.AddListener(delegate { EnterShop(); });
        bgExitUI.GetComponent<Button>().onClick.AddListener(delegate { ExitShop(); });
    }

    // Update is called once per frame
    void Update()
    {
        //Countdown timers
        #region
        if (f_lockTimer_1 >= 0f && isCounted_1 == false)
        {
            f_lockTimer_1 -= Time.deltaTime;
            switch (language)
            {
                case "ru":
                    lockTimerText_1.text = "Появится через:" + "\n" + Math.Round(f_lockTimer_1).ToString();
                    break;

                case "en":
                    lockTimerText_1.text = "Appears in:" + "\n" + Math.Round(f_lockTimer_1).ToString();
                    break;

                case "tr":
                    lockTimerText_1.text = "Şurada görünür:" + "\n" + Math.Round(f_lockTimer_1).ToString();
                    break;
            }
        }
        else if(f_lockTimer_1 <= 0f && isCounted_1 == false)
        {
            isCounted_1 = true;

            lockOrder_UI_1.SetActive(false);
            acceptButton_1.SetActive(true);

            AssignNewOrder(0);
        }

        if (f_lockTimer_2 >= 0f && isCounted_2 == false)
        {
            f_lockTimer_2 -= Time.deltaTime;
            switch (language)
            {
                case "ru":
                    lockTimerText_2.text = "Появится через:" + "\n" + Math.Round(f_lockTimer_2).ToString();
                    break;

                case "en":
                    lockTimerText_2.text = "Appears in:" + "\n" + Math.Round(f_lockTimer_2).ToString();
                    break;

                case "tr":
                    lockTimerText_2.text = "Şurada görünür:" + "\n" + Math.Round(f_lockTimer_2).ToString();
                    break;
            }
        }
        else if (f_lockTimer_2 <= 0f && isCounted_2 == false)
        {
            isCounted_2 = true;

            lockOrder_UI_2.SetActive(false);
            acceptButton_2.SetActive(true);

            AssignNewOrder(1);
        }

        if (f_lockTimer_3 >= 0f && isCounted_3 == false)
        {
            f_lockTimer_3 -= Time.deltaTime;
            switch (language)
            {
                case "ru":
                    lockTimerText_3.text = "Появится через:" + "\n" + Math.Round(f_lockTimer_3).ToString();
                    break;

                case "en":
                    lockTimerText_3.text = "Appears in:" + "\n" + Math.Round(f_lockTimer_3).ToString();
                    break;

                case "tr":
                    lockTimerText_3.text = "Şurada görünür:" + "\n" + Math.Round(f_lockTimer_3).ToString();
                    break;
            }
        }
        else if (f_lockTimer_3 <= 0f && isCounted_3 == false)
        {
            isCounted_3 = true;

            lockOrder_UI_3.SetActive(false);
            acceptButton_3.SetActive(true);

            AssignNewOrder(2);
        }

        if(shopDelay >= 0)
        {
            shopDelay -= Time.deltaTime;
        }
        #endregion
    }

    #region Yandex
    // Подписываемся на событие GetDataEvent в OnEnable
    private void OnEnable() => YandexGame.GetDataEvent += GetData;

    // Отписываемся от события GetDataEvent в OnDisable
    private void OnDisable() => YandexGame.GetDataEvent -= GetData;

    public void GetData()
    {
        // Получаем данные из плагина и делаем с ними что хотим
        language = YandexGame.EnvironmentData.language;
    }
    #endregion

    // Shop Trigger
    #region
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (PlayerPrefs.GetInt("isMobile", 0) == 0)
            {
                // Press E if you want to enter UI
                enterShopText.SetActive(true);
            }
            else
            {
                enterShopButton.SetActive(true);
            }
        }
    }
    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E) && !isInShop && shopDelay <= 0f)
            {
                EnterShop();
            }
            if (Input.GetKeyDown(KeyCode.E) && isInShop && shopDelay <= 0f)
            {
                ExitShop();
            }
            if(!isInShop && shopDelay <= 0f)
                enterShopButton.GetComponent<Button>().onClick.AddListener(delegate { EnterShop(); });
            if (isInShop && shopDelay <= 0f)
                bgExitUI.GetComponent<Button>().onClick.AddListener(delegate { ExitShop(); });
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            ExitShop();
        }
    }

    private void EnterShop()
    {
        isInShop = true;

        // Shop UI
        bgExitUI.SetActive(true);
        shopWindowUI.SetActive(true);

        // Load all orders
        LoadShopUI(3);

        shopDelay = .1f;

        // Cursor kostyl
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void ExitShop()
    {
        enterShopText.SetActive(false);
        enterShopButton.SetActive(false);

        isInShop = false;

        bgExitUI.SetActive(false);
        shopWindowUI.SetActive(false);

        UnLoadShopUI(3);

        shopDelay = .1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    #endregion


    // Check null to prevent error
    private void LoadShopUI(int orderIndex)
    {
        ReloadButtons();

        switch (orderIndex)
        {
            case 0:
                displayScript_1.currentShop = gameObject;
                displayScript_1.SetObject(0, language);
                break;

            case 1:
                displayScript_2.currentShop = gameObject;
                displayScript_2.SetObject(1, language);
                break;

            case 2:
                displayScript_3.currentShop = gameObject;
                displayScript_3.SetObject(2, language);
                break;

            case 3:
                if (thisShopOrders[0] != null)
                {
                    displayScript_1.currentShop = gameObject;
                    displayScript_1.SetObject(0, language);
                }
                if (thisShopOrders[1] != null)
                {
                    displayScript_2.currentShop = gameObject;
                    displayScript_2.SetObject(1, language);
                }
                if (thisShopOrders[2] != null)
                {
                    displayScript_3.currentShop = gameObject;
                    displayScript_3.SetObject(2, language);
                }
                break;

            default:
                Debug.LogError("LoadShopUI() orderIndex undefined");
                break;
        }
    }

    private void UnLoadShopUI(int orderIndex)
    {
        switch (orderIndex)
        {
            case 0:
                orderDisplay_1 = null;
                break;

            case 1:
                orderDisplay_2 = null;
                break;

            case 2:
                orderDisplay_3 = null;
                break;

            case 3:
                orderDisplay_1 = null;
                orderDisplay_2 = null;
                orderDisplay_3 = null;
                break;

            default:
                Debug.LogError("UnLoadShopUI() orderIndex undefined");
                break;
        }
    }


    private void AssignNewOrder(int orderIndex)
    {
        string tag = gameObject.tag;
        thisShopOrders[orderIndex] = OrderManager.InitShopOrder(tag);

        if (isInShop)
        {
            LoadShopUI(orderIndex);
        }
    }

    private void ReloadButtons()
    {
        acceptButton_1.GetComponent<Button>().onClick.RemoveAllListeners();
        acceptButton_2.GetComponent<Button>().onClick.RemoveAllListeners();
        acceptButton_3.GetComponent<Button>().onClick.RemoveAllListeners();

        acceptButton_1.GetComponent<Button>().onClick.AddListener(Take_1_order);
        acceptButton_2.GetComponent<Button>().onClick.AddListener(Take_2_order);
        acceptButton_3.GetComponent<Button>().onClick.AddListener(Take_3_order);
    }


    // Order buttons scripts
    #region
    public void Take_1_order()
    {
        if (Stats.currentWeight + thisShopOrders[0].weight > Stats.maxWeight)
        {
            Stats.Overload(0);
        }
        else
        {
            // Add weight and reward to player lists
            Stats.List_of_weight.Add(thisShopOrders[0].weight);
            Stats.RecalcWeight();
            Stats.List_of_reward.Add(thisShopOrders[0].rewardValue);

            // In this object set list[0] not assigned without index shift
            thisShopOrders[0] = null;
            displayScript_1.UnSetObject();

            // Launch timer
            f_lockTimer_1 = locktime;
            isCounted_1 = false;

            // Disable button, enable lock timer
            acceptButton_1.SetActive(false);
            lockOrder_UI_1.SetActive(true);

            UnLoadShopUI(0);

            OrderManager.PlayerNewCustomer();
        }
    }
    public void Take_2_order()
    {
        if (Stats.currentWeight + thisShopOrders[1].weight > Stats.maxWeight)
        {
            Stats.Overload(1);
        }
        else
        {
            // Add weight and reward to player lists
            Stats.List_of_weight.Add(thisShopOrders[1].weight);
            Stats.RecalcWeight();
            Stats.List_of_reward.Add(thisShopOrders[1].rewardValue);

            // In this object set list[0] not assigned without index shift
            thisShopOrders[1] = null;
            displayScript_2.UnSetObject();

            // Launch timer
            f_lockTimer_2 = locktime;
            isCounted_2 = false;

            // Disable button, enable lock timer
            acceptButton_2.SetActive(false);
            lockOrder_UI_2.SetActive(true);

            UnLoadShopUI(1);

            OrderManager.PlayerNewCustomer();
        }
    }
    public void Take_3_order()
    {
        if (Stats.currentWeight + thisShopOrders[2].weight > Stats.maxWeight)
        {
            Stats.Overload(2);
        }
        else
        {
            // Add weight and reward to player lists
            Stats.List_of_weight.Add(thisShopOrders[2].weight);
            Stats.RecalcWeight();
            Stats.List_of_reward.Add(thisShopOrders[2].rewardValue);

            // In this object set list[0] not assigned without index shift
            thisShopOrders[2] = null;
            displayScript_3.UnSetObject();

            // Launch timer
            f_lockTimer_3 = locktime;
            isCounted_3 = false;

            // Disable button, enable lock timer
            acceptButton_3.SetActive(false);
            lockOrder_UI_3.SetActive(true);

            UnLoadShopUI(2);

            OrderManager.PlayerNewCustomer();
        }
    }
    #endregion

}
