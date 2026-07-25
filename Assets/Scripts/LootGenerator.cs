using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using System.Linq;
using System;
using Random = UnityEngine.Random; 


public class LootGenerator : MonoBehaviour
{
    List<ItemData> itemList = new List<ItemData>();
    [SerializeField] public GameObject baseItemPrefab;
    
    void Start()
    {
        StartCoroutine(LoadAllItems());
    }
    public GameObject GetLootItem(GameObject parentObject)
    {
        // default to basic room if no rooms (fallback)
        GameObject baseItem = Instantiate(baseItemPrefab, parentObject.transform);
        // Randomly select a loot item for a chest
        ItemData itemData = itemList[Random.Range(0, itemList.Count)];

        InstantiateItem(baseItem, itemData);
        

        return baseItem;
    }

    public GameObject GetItem(string nameToFind, GameObject parentObject = null)
    {

        foreach(ItemData itemData in itemList)
        {
            if (string.Equals(nameToFind, itemData.item_name, StringComparison.OrdinalIgnoreCase))
            {
                GameObject baseItem = null;
                if (parentObject != null)
                {
                    baseItem = Instantiate(baseItemPrefab, parentObject.transform);
                }
                else
                {
                    baseItem = Instantiate(baseItemPrefab);
                }

                InstantiateItem(baseItem, itemData);
                return baseItem;
            }
        }
        Debug.LogWarning($"Item '{nameToFind}' not found in itemList");
        return null;
    }

    void InstantiateItem(GameObject item, ItemData itemData)
    {
        var itemBeh = item.GetComponent<ItemBehavior>();
            
        itemBeh.itemName = itemData.item_name;
        itemBeh.value = itemData.value;
        itemBeh.stackable = itemData.stackable;
    }

    IEnumerator LoadAllItems()
    {
        string indexPath = $"{Application.streamingAssetsPath}/Items/items_index.json";
        
        using (UnityWebRequest request = UnityWebRequest.Get(indexPath))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to load item index: {request.error}");
                yield break;
            }

            string rawIndexText = request.downloadHandler.text;
            
            IndexData indexData = JsonUtility.FromJson<IndexData>(rawIndexText);
            foreach (string fileName in indexData.filenames)
            {
                string itemPath = $"{Application.streamingAssetsPath}/Items/{fileName}";
                
                using (UnityWebRequest itemRequest = UnityWebRequest.Get(itemPath))
                {
                    yield return itemRequest.SendWebRequest();

                    if (itemRequest.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogWarning($"Skipped item file '{fileName}': {itemRequest.error}");
                        continue;
                    }

                    string rawItemText = itemRequest.downloadHandler.text;
                    ItemData itemData = JsonUtility.FromJson<ItemData>(rawItemText);

                    itemList.Add(itemData);
                }
            }
        }
    }
}
