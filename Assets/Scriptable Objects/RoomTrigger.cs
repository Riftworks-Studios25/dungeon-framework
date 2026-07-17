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
        GameObject nextRoom = roomManager.GetNextRoom();

        Vector2 pos = new Vector2(parentRoom.transform.position.x + nextX,
                                  parentRoom.transform.position.y + nextY);

        nextRoom.transform.position = pos;
        nextRoom.GetComponent<RoomObject>().Initialize(parentRoom);

        roomManager.UpdateRoom(nextRoom);
    }

    public float GetNextX() => nextX;
    public float GetNextY() => nextY;
}