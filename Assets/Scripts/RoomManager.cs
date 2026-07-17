using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    static public GameObject currentRoom;
    private SmoothCameraFollow cam;
    private GameObject player;
    static public int seed;
    private List<GameObject> rooms;
    private List<GameObject> previousRooms;
    void Start()
    {
        rooms = new List<GameObject>();
        previousRooms = new List<GameObject>();
        Random.InitState((int)System.DateTime.Now.Ticks);
        seed = (int)System.DateTime.Now.Ticks;
        currentRoom = GameObject.FindGameObjectWithTag("Room");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothCameraFollow>();
        player = GameObject.FindGameObjectWithTag("Player");
        
        GameObject[] roomArray = Resources.LoadAll<GameObject>("Prefabs/Rooms");
        rooms.AddRange(roomArray);
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothCameraFollow>();
    }

    public void UpdateRoom(GameObject newRoom)
    {
        currentRoom = newRoom;
        cam.UpdateTarget(newRoom.transform);
    }

    public GameObject GetNextRoom()
    {
        // Choose a random room. If a room has been picked in the last 3 rooms, it cannot be picked.
        int roomIndex = Random.Range(0, rooms.Count);
        GameObject Room = rooms[roomIndex];
        previousRooms.Add(rooms[roomIndex]);
        rooms.Remove(rooms[roomIndex]);

        if (previousRooms.Count > 2)
        {
            rooms.Add(previousRooms[0]);
            previousRooms.Remove(previousRooms[0]);
        }
        return Room;
    }
    
}
