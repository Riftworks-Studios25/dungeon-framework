using UnityEngine;

public class PressurePlateBehavior : UnlockerBehavior
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player is within the collider of the pressure plate, push it
        unlocker.Activate();
        mainUnlocker.GetComponent<UnlockerBehavior>().Unlock();
        transform.localScale = new Vector3(.9f, .9f, 1);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        unlocker.Deactivate();
        mainUnlocker.GetComponent<UnlockerBehavior>().Unlock();
        transform.localScale = new Vector3(1, 1, 1);
    }
}
