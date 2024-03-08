using UnityEngine;

[CreateAssetMenu(fileName = "New order", menuName = "Orders")]
public class OrderObject : ScriptableObject
{
    public string ruName;
    public string enName; 
    public string trName;

    public float weight;

    public int rewardValue;
}
