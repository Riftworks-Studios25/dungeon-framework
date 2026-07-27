using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using System.Linq;
using System;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
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
        string indexPath = "";
        if (itemData.random_sprite)
        {
            indexPath = $"{Application.streamingAssetsPath}/Items/Sprites/{itemData.sprite_filename}";
            StartCoroutine(RandomSprite(indexPath, (returnedPath) =>
            {
                indexPath = $"{Application.streamingAssetsPath}/Items/Sprites/{returnedPath}";
                StartCoroutine(GetSprite(indexPath, item, itemData));
            }));
        }
        else
        {
            indexPath = $"{Application.streamingAssetsPath}/Items/Sprites/{itemData.sprite_filename}";
            StartCoroutine(GetSprite(indexPath, item, itemData));
        }
    }

    IEnumerator LoadAllItems()
    {
        string indexPath = $"{Application.streamingAssetsPath}/Items/items_index.json";

        using UnityWebRequest request = UnityWebRequest.Get(indexPath);
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

    IEnumerator GetSprite(string indexPath, GameObject item, ItemData itemData)
    {
        using UnityWebRequest request = UnityWebRequestTexture.GetTexture(indexPath);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load: " + request.error + "from " + indexPath);
        }

        byte[] rawData = request.downloadHandler.data;
        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);

        if (tex.LoadImage(rawData))
        {
            tex.filterMode = FilterMode.Point;
            tex.wrapMode = TextureWrapMode.Clamp;
            Sprite fromTex = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), itemData.pixels_per_unit, 0, SpriteMeshType.Tight);
            
            if (item != null)
            {
                item.GetComponent<SpriteRenderer>().sprite = fromTex;
                item.GetComponent<Image>().sprite = fromTex;
            }
        }
        else
        {            
            Debug.LogError($"Failed to load texture at path: {indexPath}");
            Addressables.LoadAssetAsync<Sprite>("Assets/Sprites/missing_texture.png").Completed += (handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    item.GetComponent<SpriteRenderer>().sprite = handle.Result;
                    item.GetComponent<Image>().sprite = handle.Result;
                }
                else
                {
                    Debug.LogError($"Failed to load the missing texture sprite. We're cooked.");
                }
            };
            
        }
    }

    IEnumerator RandomSprite(string indexPath, System.Action<string> callback)
    {
        using UnityWebRequest request = UnityWebRequest.Get(indexPath);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Failed to load item index: {request.error}");
            yield break;
        }

        string rawIndexText = request.downloadHandler.text;

        IndexData indexData = JsonUtility.FromJson<IndexData>(rawIndexText);
        if (indexData.filenames.Count > 0)
        {
            callback?.Invoke(indexData.filenames[Random.Range(0, indexData.filenames.Count)]);
        }
        else
        {
            callback?.Invoke("null");
        }
    }
}
