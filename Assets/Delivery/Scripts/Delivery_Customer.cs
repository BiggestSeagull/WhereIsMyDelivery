using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Delivery_Customer : MonoBehaviour
{
    private Delivery_OrderManager OrderManager;
    private Delivery_Stats Stats;
    private Ui_City UiCity;
    
    // Start is called before the first frame update
    void Start()
    {
        OrderManager = GameObject.FindWithTag("GameManager").GetComponent<Delivery_OrderManager>();
        Stats = GameObject.FindWithTag("Player").GetComponent<Delivery_Stats>();
        UiCity = GameObject.FindWithTag("GameManager").GetComponent<Ui_City>();
    }

    private void OnTriggerEnter(Collider col)
    {
        // If trigger is garage
        if (gameObject.CompareTag("Delivery_Garage") && col.CompareTag("Player"))
        {
            UiCity.triggerGoGarageBlock.SetActive(true);
            Ui_City.UnlockCursor();
            return;
        }

        CompleteOrder();
    }
    private void OnTriggerExit(Collider col)
    {
        if (gameObject.CompareTag("Delivery_Garage"))
        {
            UiCity.triggerGoGarageBlock.SetActive(false);
            Ui_City.LockCursor();
        }
    }

    private void CompleteOrder()
    {
        // Find all matching customers in the list of taken orders
        List<GameObject> matchingCustomers = new();
        matchingCustomers = OrderManager.playerCustomers
            .Where(obj => obj.name == gameObject.name)
            .ToList();

        // Give to Player reward for delivery matchingCustomers.Count times
        for(int i = 0; i < matchingCustomers.Count; i++)
        {
            int randomOrder = Random.Range(0, Stats.List_of_weight.Count);

            // Payment for delivery
            YG_Saves.SaveCoins(YG_Saves.LoadCoins() + Stats.List_of_reward[randomOrder]);
            UiCity.ReloadCoins();

            // Write to temp leaderboard prefs
            PlayerPrefs.SetInt("lbVarToSave", 0);   // Reset temporary value
            PlayerPrefs.SetInt("lbVarToSave", PlayerPrefs.GetInt("lbVarToSave", 0) + 1);

            Stats.List_of_weight.RemoveAt(randomOrder);
            Stats.List_of_reward.RemoveAt(randomOrder);
        }

        // Deactivate and remove all matching customers
        foreach (GameObject customer in matchingCustomers)
        {
            customer.SetActive(false);
            OrderManager.playerCustomers.Remove(customer);
        }

        Stats.RecalcWeight();

        UiCity.CollectCoins();

        // Write to Yandex leaderboard
        Leaderboard.NewWrite();

        TimerBeforeAdsYG.shouldShowAd = true;
    }
}
