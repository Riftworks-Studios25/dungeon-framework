using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unlocker", menuName = "Scriptable Objects/Unlocker")]
public class Unlocker : ScriptableObject
{
    public bool active = false;
    public virtual void Activate()
    {
        active = true;
    }
    public virtual void Deactivate()
    {
        active = false;
    }
    
    public bool IsActive() => active;
}
