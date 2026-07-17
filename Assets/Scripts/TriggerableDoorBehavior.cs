using UnityEngine;
using UnityEngine.Rendering;

public class TriggerableDoorBehavior : TriggerableBehavior
{
    [SerializeField] public Sprite closedSprite;
    [SerializeField] public Sprite openSprite;
    private SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    public override void Toggle()
    {
        base.Toggle();
        GetComponent<BoxCollider2D>().enabled = triggerable.IsLocked();

        // Change sprite
        if (triggerable.IsLocked())
        {
            sr.sprite = closedSprite;
        }
        else
        {
            sr.sprite = openSprite;
        }
    }
}
