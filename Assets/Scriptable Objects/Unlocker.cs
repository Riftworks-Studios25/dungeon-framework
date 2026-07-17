using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unlocker", menuName = "Scriptable Objects/Unlocker")]
public class Unlocker : ScriptableObject
{
    public bool active = false;
    public virtual void Toggle()
    {
        if (active)
        {
            active = false;
        }
        else
        {
            active = true;
        }
    }
    
    public bool IsActive() => active;
}
