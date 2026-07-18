using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] public GameObject slot;
    private List<InventorySlot> slots;
    PlayerInventory playerInv;
    public InputAction scroll;
    private int activeSlot;
    void Start()
    {
        slots = new List<InventorySlot>();

        // Create slots based on Players' inv size
        playerInv = GameObject.Find("Player").GetComponent<PlayerInventory>();
        for (int i = 0; i < playerInv.inventorySize; i++)
        {
            GameObject newSlot = Instantiate(slot, transform);
            newSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(90 + 180 * i, -90);
            slots.Add(newSlot.GetComponent<InventorySlot>());
            newSlot.GetComponent<InventorySlot>().slotNum = i;
        }

        // Set the 0th slot to active at the beginning 
        slots[0].SetActive(true);
        activeSlot = 0;

        scroll.Enable();
        scroll.performed += OnScroll;
    }
    private void OnDisable()
    {
        if (scroll != null)
        {
            scroll.performed -= OnScroll;
            scroll.Disable();
        }
    }

    private void OnScroll(InputAction.CallbackContext context)
    {
        // Scroll down to go right on the inventory, up to go left
        float value = context.ReadValue<float>();

        if (value > 0f)
        {
            // Scroll right
            NextScroll();            
        }
        else if (value < 0f)
        {
            // Scroll left
            PrevScroll();
        }
    }

    private void NextScroll()
    {
        activeSlot += 1;
        if (activeSlot > playerInv.inventorySize - 1)
        {
            activeSlot = 0;
        }
        ActivateSlot(activeSlot);
    }

    private void PrevScroll()
    {
        activeSlot -= 1;
        if (activeSlot < 0)
        {
            activeSlot = playerInv.inventorySize - 1;
        }
        ActivateSlot(activeSlot);
    }

    private void ActivateSlot(int slotNum)
    {
        // Set a slot to active
        for (int i = 0; i < playerInv.inventorySize; i++)
        {
            if (i == slotNum)
            {
                slots[i].SetActive(true);
            }
            else
            {
                slots[i].SetActive(false);
            }
        }
    }

    public void ItemUpdate()
    {
        for (int i = 0; i < playerInv.items.Count(); i++)
        {
            slots[i].ItemUpdate(playerInv.items);
        }
    }

    public int GetCurrentSlot() => activeSlot;
}
