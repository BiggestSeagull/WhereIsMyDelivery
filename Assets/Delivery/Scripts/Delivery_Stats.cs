using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using YG;

public class Delivery_Stats : MonoBehaviour
{
    // player RCC
    private RCC_CarControllerV3 RCC;

    // Arrays of possible stats
    [SerializeField] private float[] engineArray;
    [SerializeField] private float[] speedArray;
    [SerializeField] private float[] capacityArray;

    // Array of price
    public int[] priceArray;

    // Weight/Capacity
    [HideInInspector] public float maxWeight;
    [HideInInspector] public float currentWeight;

    // Delivery process lists
    [HideInInspector] public List<float> List_of_weight;
    [HideInInspector] public List<int> List_of_reward;
    
    // UI
    public GameObject[] OverloadUI; // Too much weight
    public TMP_Text WeightText;     // Shows current weight

    [SerializeField] private GameObject hood;

    // Music
    [HideInInspector] public bool isMusicOn;

    // Yandex
    [HideInInspector] public string language;

    private void Awake()
    {
        RCC = GameObject.FindWithTag("Player").GetComponent<RCC_CarControllerV3>();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 | PlayerPrefs.GetInt("transFromMainScene", 0) == 1)
        {
            PlayerPrefs.SetInt("transFromMainScene", 0);
            GetData();
        }
    }

    // To rewrite RCC
    IEnumerator LoadStats()
    {
        yield return new WaitForEndOfFrame();
        RCC.maxEngineTorque = engineArray[YG_Saves.LoadEngine()];
        RCC.maxspeed = speedArray[YG_Saves.LoadSpeed()];
        maxWeight = (float)(Math.Round(capacityArray[YG_Saves.LoadCapacity()] * 100) / 100);

        if (SceneManager.GetActiveScene().buildIndex == 1)
            RecalcWeight();
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

        if (PlayerPrefs.GetInt("music", 1) == 1)
        {
            isMusicOn = true;
        }
        else
        {
            isMusicOn = false;
        }

        // Load stats
        StartCoroutine(LoadStats());
    }
    #endregion

    // Delivery process script
    public void RecalcWeight()
    {
        currentWeight = 0;

        for (int i = 0; i < List_of_weight.Count; i++)
        {
            currentWeight += List_of_weight[i];
        }

        switch (language)
        {
            case "ru":
                WeightText.text = "Масса заказов: \n" + currentWeight.ToString() + " / " + maxWeight.ToString() + " кг";
                return;

            case "en":
                WeightText.text = "Weight of orders: \n" + currentWeight.ToString() + " / " + maxWeight.ToString() + " kg";
                return;

            case "tr":
                WeightText.text = "Siparişlerin kütlesi: \n" + currentWeight.ToString() + " / " + maxWeight.ToString() + " kg";
                return;
        }

    }

    // Delivery process script
    public void Overload(int index)
    {
        OverloadUI[index].SetActive(true);
        StartCoroutine(TurnOff(index));
    }
    IEnumerator TurnOff(int index)
    {
        yield return new WaitForSeconds(2f);
        OverloadUI[index].SetActive(false);
    }

    // Upgrade process script
    public void UpgradeTierOf(int statIndex)
    {
        switch (statIndex)
        {
            case 0:
                YG_Saves.SaveEngine(YG_Saves.LoadEngine() + 1);
                break;
            case 1:
                YG_Saves.SaveSpeed(YG_Saves.LoadSpeed() + 1);
                break;
            case 2:
                YG_Saves.SaveCapacity(YG_Saves.LoadCapacity() + 1);
                break;
        }
    }

    // Upgrade process script
    public void DowngradeTierOf(int statIndex)
    {
        switch (statIndex)
        {
            case 0:
                YG_Saves.SaveEngine(YG_Saves.LoadEngine() - 1);
                break;
            case 1:
                YG_Saves.SaveSpeed(YG_Saves.LoadSpeed() - 1);
                break;
            case 2:
                YG_Saves.SaveCapacity(YG_Saves.LoadCapacity() - 1);
                break;
        }
    }
}
