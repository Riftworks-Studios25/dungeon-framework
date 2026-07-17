using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Trigger", menuName = "Scriptable Objects/RoomTrigger")]
public class RoomTrigger : Trigger
{
    [SerializeField] private float nextX;
    [SerializeField] private float nextY;

    public override void Activate(GameObject parentRoom)
    {
        RoomManager roomManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<RoomManager>();
        GameObject Room = roomManager.GetNextRoom();

        Vector2 pos = new Vector2(parentRoom.transform.position.x + nextX,
                                  parentRoom.transform.position.y + nextY);

        GameObject nextRoom = Instantiate(Room, pos, Quaternion.identity);
        nextRoom.GetComponent<RoomObject>().Initialize(parentRoom);

        roomManager.UpdateRoom(nextRoom);
    }

    public float GetNextX() => nextX;
    public float GetNextY() => nextY;
}