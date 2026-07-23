using UnityEngine;
using UnityEngine.InputSystem;

public class LeverBehavior : UnlockerBehavior, IRotateFixable
{
    private bool playerInRange = false;
    private bool isSubscribed = false;
    public InputAction activate;
    public bool rotateFix { get; set; } = true;
    private void OnDisable()
    {
        // Ensure we clean up if this object is disabled while subscribed
        if (isSubscribed && activate != null)
        {
            activate.performed -= OnActivate;
            activate.Disable();
            isSubscribed = false;
        }
    }

    private void OnActivate(InputAction.CallbackContext context)
    {
        if (playerInRange)
        {
            if (unlocker.IsActive())
            {
                unlocker.Deactivate();
            }
            else
            {
                unlocker.Activate();
            }
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
                activate.performed += OnActivate;
                isSubscribed = true;
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
                activate.performed -= OnActivate;
                activate.Disable();
                isSubscribed = false;
            }
        }
    }
}
