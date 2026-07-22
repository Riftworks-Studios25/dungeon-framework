using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "Triggerable", menuName = "Scriptable Objects/Triggerable")]

public class Triggerable : ScriptableObject
{
    public bool locked = true;

    public virtual void Unlock()
    {
        locked = false;
    }
    public virtual void Lock()
    {
        locked = true;
    }

    public Triggerable CreateRuntimeInstance()
    {
        return Instantiate(this);
    }

    public bool IsLocked() => locked;
}
