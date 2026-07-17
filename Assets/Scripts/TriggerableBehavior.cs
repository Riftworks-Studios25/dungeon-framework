using UnityEngine;

public class TriggerableBehavior : MonoBehaviour
{
    public Triggerable triggerable;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        triggerable = triggerable.CreateRuntimeInstance();
    }

    public virtual void Toggle()
    {
        triggerable.Toggle();
    }
    
    public void Unlock()
    {
        if (IsLocked())
        {
            Toggle();
        }
    }
    public void Lock()
    {
        if (!IsLocked())
        {
            Toggle();
        }
    }

    public bool IsLocked() => triggerable.IsLocked();
}
