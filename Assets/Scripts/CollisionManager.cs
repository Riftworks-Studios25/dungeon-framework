using System;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{

    RoomManager roomManager;
    void Update()
    {
        // If the roomManager can't be found, FIND IT AT ALL COSTS
        if (roomManager == null)
        {
            try {roomManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<RoomManager>();} catch{}
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Collision with triggers such as creating new rooms and switching to the next room
        if (collision.gameObject.CompareTag("Trigger"))
        {
            TriggerCollide triggerCollide = collision.GetComponent<TriggerCollide>();
            if (triggerCollide != null)
            {
                triggerCollide.Collide();
            }
        }
        // When you collide with a room, switch to it in the roomManager
        else if (collision.gameObject.CompareTag("Room"))
        {
            if (collision.gameObject != RoomManager.currentRoom)
            {
                roomManager.UpdateRoom(collision.gameObject);
            }
        }

    }
    void OnCollisionExit2D(Collision2D collision)
    {
    // Box push
    if (collision.gameObject.tag == "Box")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
        }

    }
}
