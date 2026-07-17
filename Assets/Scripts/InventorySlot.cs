using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class InventorySlot : MonoBehaviour
{
    public bool isActive = false;
    [SerializeField] public Sprite slotSprite;
    [SerializeField] public Sprite selectedSprite;
    private UnityEngine.UI.Image spriteRenderer;
    public int slotNum;
    
    void Awake()
    {
        spriteRenderer = GetComponent<UnityEngine.UI.Image>();
    }
    public void SetActive(bool set)
    {
        isActive = set;
        if (isActive)
        {
            spriteRenderer.sprite = selectedSprite;
        }
        else
        {
            spriteRenderer.sprite = slotSprite;
        }
    }

    public void ItemUpdate(List<GameObject> inv)
    {
        // Make sure item is a part of the canvas and is in the correct inv position
        GameObject slotItem = inv[slotNum];
        slotItem.transform.SetParent(transform.parent);
        slotItem.transform.position = transform.position;
        slotItem.transform.localScale = new UnityEngine.Vector3(64, 64, 0);
        slotItem.transform.rotation = transform.rotation;
    }
}
