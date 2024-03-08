using UnityEngine;
using TMPro;

public class OrderDisplay : MonoBehaviour
{
    public Delivery_Shop Shop;

    public GameObject currentShop;

    public OrderObject orderObject;

    public TMP_Text orderName;
    public TMP_Text orderWeight;


    public void SetObject(int index, string lang)
    {
        Shop = currentShop.GetComponent<Delivery_Shop>();
        orderObject = Shop.thisShopOrders[index];

        switch (lang)
        {
            case "ru":
                orderName.text = orderObject.ruName;
                orderWeight.text = "масса: " + orderObject.weight.ToString() + " кг";
                break;

            case "en":
                orderName.text = orderObject.enName;
                orderWeight.text = "weight: " + orderObject.weight.ToString() + " kg";
                break;

            case "tr":
                orderName.text = orderObject.trName;
                orderWeight.text = "ağırlığı: " + orderObject.weight.ToString() + " kg";
                break;
        }

    }
    public void UnSetObject()
    {
        orderObject = null;
        orderName.text = "";
        orderWeight.text = "";

    }
}
