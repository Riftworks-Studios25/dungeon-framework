using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopKeeper : MonoBehaviour
{
    public YesNoUI ui;
    private string questionText;
    private bool active = true;
    public GameObject coin;
    private readonly PlayerInventory inv;

    void OnTriggerExit2D(Collider2D collision)
    {
        active = true;
    }
    public void Exchange(PlayerInventory inv)
    {
        // Get coins in exchange for your loot
        int coinCount = inv.GetPlayerWorth();
        if (coinCount > 0)
        {
            GameObject newCoin = Instantiate(coin);
            newCoin.GetComponent<ItemBehavior>().stackCount = coinCount;
            inv.Wipe();
            inv.PickupItem(newCoin);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {   
        if (collision.gameObject.CompareTag("Player") && active)
        {
            PlayerInventory inv = collision.GetComponent<PlayerInventory>();
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            bool firstTime = true;
            // Check if the persistent canvas exists (only exists after you've first entered the dungeon)
            foreach (Canvas canvas in canvases)
            {
                if (canvas.gameObject.name == "Persistent Canvas")
                {
                    firstTime = false;
                    break;
                }
                else{firstTime = true;}
            }
            if (firstTime)
            {
                ui.ShowYesNo("Welcome to The Dungeon! Walk over to that big purple portal to enter the dungeon and bring me goodies!", answer =>{});
                active = false;
            }
            else if (inv.items.Count == 1 && inv.items[0].GetComponent<ItemBehavior>().GetName() == "Coin")
            {
                ui.ShowYesNo("Enhance your movement speed? (10 Coins)", answer =>
                {
                    if (answer)
                    {
                        if (inv.GetPlayerWorth() >= 10)
                        {
                            FindAnyObjectByType<PlayerController>().AddSpeed(.1f);
                            foreach (GameObject item in inv.items)
                            {
                                if (item.GetComponent<ItemBehavior>().GetName() == "Coin")
                                {
                                    item.GetComponent<ItemBehavior>().StackIncrease(-10);
                                }
                            }
                        }
                        else
                        {
                            ui.ShowYesNo("Sorry! Enter the dungeon to get more coins!", answer =>{});
                        }
                    }
                });
            }
            else
            {
                ui.ShowYesNo("Exchange all your loot for coins?", answer =>
                {
                    if (answer)
                    {
                        Exchange(inv);
                        active = false;
                    }
                });
            }
        }
    }
}
