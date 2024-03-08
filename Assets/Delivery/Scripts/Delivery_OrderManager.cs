using System.Collections.Generic;
using UnityEngine;

public class Delivery_OrderManager : MonoBehaviour
{
    [SerializeField] Ui_City UiCity;

    // Customers
    public GameObject[] customers;

    // Shops
    public GameObject[] shops_FastFood;
    public GameObject[] shops_Flowers;
    public GameObject[] shops_Grocery;
    public GameObject[] shops_Bank;

    // Orders
    public OrderObject[] orders_FastFood;
    public OrderObject[] orders_Flowers;
    public OrderObject[] orders_Grocery;
    public OrderObject[] orders_Bank;

    // Assigned Customers
    public List<GameObject> playerCustomers = new();
    private Transform navigationArrow;
    private GameObject player;

    // Navigation arrow LookAt
    private float closestDistance; // Variable to store the closest distance
    private Transform nearestCustomer; // Reference to the nearest customer's transform

    // Go garage
    [HideInInspector] public bool isGoGarage;
    [SerializeField] private Transform garageTrigger;


    // Start is called before the first frame update
    void Start()
    {
        // Some defines
        player = GameObject.FindWithTag("Player");
        navigationArrow = player.transform.GetChild(1);

        InitOrders();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGoGarage)
            PlayerNearestCustomer();
        else
            ArrowToGarage();
    }

    // Init orders depending on type of shop on start
    private void InitOrders()
    {
        AssignOrders(shops_FastFood, orders_FastFood);
        AssignOrders(shops_Flowers, orders_Flowers);
        AssignOrders(shops_Grocery, orders_Grocery);
        AssignOrders(shops_Bank, orders_Bank);
    }

    private void AssignOrders(GameObject[] shops, OrderObject[] orders)
    {
        foreach (GameObject shop in shops)
        {
            for (int i = 0; i < 3; i++)
            {
                int randomIndex = Random.Range(0, orders.Length);
                shop.GetComponent<Delivery_Shop>().thisShopOrders.Add(orders[randomIndex]);
            }
        }
    }

    // Init individual shop`s order
    // Called from above
    public OrderObject InitShopOrder(string shopTag)
    {
        switch (shopTag)
        {
            case "Delivery_isFastFood":
                return orders_FastFood[Random.Range(0, orders_FastFood.Length)];
            case "Delivery_isFlowers":
                return orders_Flowers[Random.Range(0, orders_Flowers.Length)];
            case "Delivery_isGrocery":
                return orders_Grocery[Random.Range(0, orders_Grocery.Length)];
            case "Delivery_isBank":
                return orders_Bank[Random.Range(0, orders_Bank.Length)];
            default:
                Debug.LogError("incorrect shop type string. from Delivery_OrderManager");
                return null;
        }

    }

    // Call this when taking order
    public void PlayerNewCustomer()
    {
        GameObject CustomerToAdd = customers[Random.Range(0, customers.Length)];

        playerCustomers.Add(CustomerToAdd);

        CustomerToAdd.SetActive(true);

        PlayerNearestCustomer();
    }
    
    // Updating nav arrow
    public void PlayerNearestCustomer()
    {
        // NavArrow setting active
        if (playerCustomers != null) navigationArrow.gameObject.SetActive(true);
        else navigationArrow.gameObject.SetActive(false);

        closestDistance = float.MaxValue;

        foreach (GameObject customer in playerCustomers)
        {
            // Calculate the distance between the player and the customer
            float distance = Vector3.Distance(player.transform.position, customer.transform.position);

            // Check if this customer is closer than the previously closest customer
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestCustomer = customer.transform;
            }
        }

        navigationArrow.transform.LookAt(nearestCustomer);

        if (playerCustomers.Count == 0)
        {
            navigationArrow.gameObject.SetActive(false);
        }
        else
        {
            navigationArrow.gameObject.SetActive(true);
        }
    }

    public void ArrowToGarage()
    {
        navigationArrow.gameObject.SetActive(true);
        navigationArrow.transform.LookAt(garageTrigger);
    }
}
