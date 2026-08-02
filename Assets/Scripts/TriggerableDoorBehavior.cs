using UnityEngine;
using UnityEngine.Rendering;

public class TriggerableDoorBehavior : TriggerableBehavior
{
    [SerializeField] public Sprite closedSprite;
    [SerializeField] public Sprite openSprite;
    private SpriteRenderer sr;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    public override void Unlock()
    {
        base.Unlock();
        GetComponent<BoxCollider2D>().enabled = triggerable.IsLocked();

        // Change sprite
        sr.sprite = openSprite;
    }
    public override void Lock()
    {
        base.Lock();
        GetComponent<BoxCollider2D>().enabled = triggerable.IsLocked();

        // Change sprite
        sr.sprite = closedSprite;
    }
}
