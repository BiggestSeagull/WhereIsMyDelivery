using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Delivery_Immersive: MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private Transform[] coinsTransform;    // Pos in the canvas

    private List<GameObject> instantiatedCoins = new();     // Here coins adding/deleting from script
    [SerializeField] private Transform parentToInst;    // Place in hierarcy to spawn coins

    [SerializeField] private Transform playerCoins;     // UI elem where coin are going

    // Play anim
    public void CollectCoins()
    {
        // Inst and dding coins to list
        for(int i = 0; i < coinsTransform.Length; i++)
        {
            GameObject instCoins =  Instantiate(coinPrefab, coinsTransform[i].position, Quaternion.identity, parentToInst);
            instantiatedCoins.Add(instCoins);
        }

        // Do anim and destroy coins
        foreach (GameObject coin in instantiatedCoins)
        {
            Vector3 targetPos = new(coin.transform.position.x, coin.transform.position.y - 30f, coin.transform.position.z);
            coin.transform.DOMove(targetPos, .7f).SetEase(Ease.InQuint).OnComplete(() =>{
            coin.transform.DOMove(playerCoins.position, 1.5f).SetEase(Ease.InOutQuad).OnComplete(() =>{
            instantiatedCoins.Remove(coin); Destroy(coin);});});
        }
    }
}
