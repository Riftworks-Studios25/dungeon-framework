using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerInventory : MonoBehaviour
{
    public int inventorySize = 4;
    public List<GameObject> items;
    private InventoryUI inventoryUI;
    void Start()
    {
        items = new List<GameObject>();
        inventoryUI = FindAnyObjectByType<InventoryUI>();
    }

    void Update()
    {
        if (inventoryUI == null)
        {
            inventoryUI = FindAnyObjectByType<InventoryUI>();
        }
    }

    public void PickupItem(GameObject item)
    {
        // Item pickup logic
        ItemBehavior itemBehavior = item.GetComponent<ItemBehavior>();
        bool found = false;
        if (itemBehavior.IsStackable())
        {
            foreach(GameObject invItem in items)
            {
                // If the picked up item is stackable, check if it can be stacked with another item in the inv
                if (itemBehavior.GetName() == invItem.GetComponent<ItemBehavior>().GetName())
                {
                    invItem.GetComponent<ItemBehavior>().StackIncrease();
                    Destroy(item);
                    found = true;
                    break;
                }
            }
        }
        if (!found)
        {
            // If it's not stackable or it can't be stacked, replace an item or push it on the end.
            if (items.Count >= inventorySize)
            {
                int itemSlot = inventoryUI.GetCurrentSlot();
                GameObject oldItem = items[itemSlot];
                DropItem(oldItem);
                items.Insert(itemSlot, item);
            }
            else
            {
                items.Add(item);
            }
        }
        itemBehavior.inInventory = true;
        inventoryUI.ItemUpdate();
    }

    public void DropItem(GameObject item)
    {
        // Remove an item from the inventory, reset it, and place it at the players point
        items.Remove(item);
        item.transform.SetParent(null, true);
        item.GetComponent<ItemBehavior>().inInventory = false;
        item.transform.position = gameObject.transform.position;
        item.transform.localScale = new Vector3(1, 1, 1);
        inventoryUI.ItemUpdate();
    }

    public int GetPlayerWorth()
    {
        int total = 0;
        foreach (GameObject item in items)
        {
            ItemBehavior itemBehavior = item.GetComponent<ItemBehavior>();
            total += itemBehavior.GetItemWorth() * itemBehavior.stackCount;

        }
        return total;
    }

    public void Wipe()
    {
        foreach(GameObject item in items)
        {
            Destroy(item);
        }
        items = new List<GameObject>();
        inventoryUI.ItemUpdate();
    }
}
