using UnityEngine;
using UnityEngine.InputSystem;

public class LeverBehavior : UnlockerBehavior
{
    private bool playerInRange = false;
    private bool isSubscribed = false;
    public InputAction activate;
    private void OnDisable()
    {
        // Ensure we clean up if this object is disabled while subscribed
        if (isSubscribed && activate != null)
        {
            activate.started -= OnActivate;
            activate.Disable();
            isSubscribed = false;
        }
    }

    private void OnActivate(InputAction.CallbackContext context)
    {
        if (playerInRange)
        {
            unlocker.Toggle();
            transform.Rotate(new Vector3(0, 180, 0));
            toggleLight.GetComponent<ToggleIndicator>().Evaluate(unlocker.IsActive());

            mainUnlocker.GetComponent<UnlockerBehavior>().Unlock();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player is in range of the lever, they can pull it
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            // Only enable and subscribe when player enters this unlocker's range
            if (activate != null && !isSubscribed)
            {
                activate.Enable();
                activate.started += OnActivate;
                isSubscribed = true;

                // Reset the action so it won't trigger immediately
                activate.ReadValue<float>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            // Unsubscribe and disable when player leaves range
            if (activate != null && isSubscribed)
            {
                activate.started -= OnActivate;
                activate.Disable();
                isSubscribed = false;
            }
        }
    }
}
