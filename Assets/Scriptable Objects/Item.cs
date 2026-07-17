using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public int itemWorth = 0;
    public bool stackable = false;
    public new String name;
    public virtual void Use()
    {
        
    }

    public int GetItemWorth() => itemWorth;
    public bool IsStackable() => stackable;
    public String GetName() => name;
}
