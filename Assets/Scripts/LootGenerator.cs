using UnityEngine;

public class LootGenerator : MonoBehaviour
{
    public GameObject GetItem()
    {
        // Randomly select a loot item for a chest
        GameObject[] items = Resources.LoadAll<GameObject>("Prefabs/Items");
        GameObject item = items[Random.Range(0, items.Length)];
        return item;
    }
}
