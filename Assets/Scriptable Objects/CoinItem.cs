using UnityEngine;

[CreateAssetMenu(fileName = "CoinItem", menuName = "Scriptable Objects/CoinItem")]
public class CoinItem : Item
{
    void OnStart()
    {
        itemWorth = 1;
        stackable = true;
        name = "Coin";
    }
}
