using System;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerCollide : MonoBehaviour
{
    public RoomTrigger trigger;
    [SerializeField] private GameObject parentRoom;
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Room") && collision.gameObject != parentRoom)
        {
            // If trigger is colliding with a room that isn't its parent, destroy it. It means that it has already generated a room.
            Destroy(this);
        }
        else if (collision.gameObject.CompareTag("Trigger"))
        {
            // If there are two triggers in the same place we don't need that.
            Destroy(collision.gameObject);
            Destroy(this);
        }
    }

    public void Collide()
    {   
        // Generate new room when you collide with trigger
        bool active = true;
        GameObject[] allRooms = GameObject.FindGameObjectsWithTag("Room");

        foreach (GameObject room in allRooms)
        {
            if (room != parentRoom &&
                Math.Abs(room.transform.position.x - transform.position.x) < 9 &&
                Math.Abs(room.transform.position.y - transform.position.y) < 9)
            {
                Destroy(this);
                active = false;
                break;
            }
        }

        if (active)
        {
            trigger.Activate(parentRoom);
        }

        Destroy(this);
    }

    public void SetParent(GameObject room)
    {
        parentRoom = room;
    }
}