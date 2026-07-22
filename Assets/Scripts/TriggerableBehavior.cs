using UnityEngine;

public class TriggerableBehavior : MonoBehaviour
{
    public Triggerable triggerable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        triggerable = triggerable.CreateRuntimeInstance();
    }
    
    public virtual void Unlock()
    {
        if (IsLocked())
        {
            triggerable.Unlock();
        }
    }
    public virtual void Lock()
    {
        if (!IsLocked())
        {
            triggerable.Lock();
        }
    }

    public bool IsLocked() => triggerable.IsLocked();
}
