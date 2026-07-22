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
        lootItem = Instantiate(loot, transform);
        RectTransform rectTransform = lootItem.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 0.2f);
        lootItem.SetActive(false);
    }

    public override void Unlock()
    {
        base.Unlock();
        
        sr.sprite = openSprite;
        if (!inactive)
        {
            lootItem.SetActive(true);
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
                lootItem.SetActive(false);
            }
        }
    }
    public override void Lock()
    {
        base.Lock();
        
        sr.sprite = closedSprite;
        if (lootItem == null || lootItem.GetComponent<ItemBehavior>().inInventory)
        {
            inactive = true;
        }
        if (!inactive)
        {
            lootItem.SetActive(false);
        }
    }
}