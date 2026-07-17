using UnityEngine;

public class TriggerableChestBehavior : TriggerableBehavior
{
    public Sprite closedSprite;
    public Sprite openSprite;
    private GameObject spriteHolder;
    private SpriteRenderer sr;
    private LootGenerator lootGenerator;
    private GameObject loot;
    private GameObject lootItem;
    private bool inactive = false;

    void Awake()
    {
        if (spriteHolder == null)
        {
            Transform t = transform.Find("chest_sprite");
            if (t != null) spriteHolder = t.gameObject;
        }

        sr = spriteHolder.GetComponent<SpriteRenderer>();

        sr.sprite = closedSprite;
    }

    void Start()
    {
        lootGenerator = FindAnyObjectByType<LootGenerator>();
        loot = lootGenerator.GetItem();
    }

    public override void Toggle()
    {
        base.Toggle();

        // Change sprite
        if (sr.sprite == closedSprite)
        {
            sr.sprite = openSprite;
            if (!inactive)
            {
                lootItem = Instantiate(loot, transform);
            }
        }
        else
        {
            sr.sprite = closedSprite;
            if (lootItem == null || lootItem.GetComponent<ItemBehavior>().inInventory)
            {
                inactive = true;
            }
            if (!inactive)
            {
                Destroy(lootItem);
            }
        }
    }
}