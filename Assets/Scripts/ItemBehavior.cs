using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ItemBehavior : MonoBehaviour
{
    [SerializeField] Item item;
    public bool inInventory = false;
    public InputAction pickup;
    private bool playerInRange = false;
    public int stackCount = 1;
    private PlayerInventory playerInventory;
    [SerializeField] public TMP_Text text;
    void Start()
    {
        item = Instantiate(item);
        playerInventory = FindAnyObjectByType<PlayerInventory>();

        if (text != null ) {text.text = "" + stackCount;}

        pickup.Enable();
    }

    public void Use()
    {
        item.Use();
    }

        void Update()
    {
        if (playerInRange && !inInventory)
        {
            if (pickup.WasPressedThisFrame())
            {
                playerInventory.PickupItem(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void StackIncrease(int amount = 1)
    {
        // Increase stackcount
        stackCount += amount;
        if (text != null) {text.text = "" + stackCount;}
        if (stackCount == 0)
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        // Destroy the item if it's NOT in the inventory
        if (!inInventory && transform.parent == null)
        {
            Destroy(gameObject);
        }
    }

    public int GetItemWorth() => item.GetItemWorth();
    public bool IsStackable() => item.IsStackable();
    public String GetName() => item.GetName();
}
