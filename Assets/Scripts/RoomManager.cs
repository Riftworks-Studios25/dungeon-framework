using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using UnityEngine.Networking;

public class RoomManager : MonoBehaviour
{
    static public GameObject currentRoom;
    private SmoothCameraFollow cam;
    private GameObject player;
    static public int seed;
    private List<RoomData> previousRooms = new List<RoomData>();
    List<RoomData> roomPool = new List<RoomData>();
    List<RoomData> roomList = new List<RoomData>();
    public List<RoomPrefabMapping> prefabMappings = new List<RoomPrefabMapping>();
    [SerializeField] public GameObject baseRoomPrefab;
    void Start()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        seed = (int)System.DateTime.Now.Ticks;
        currentRoom = GameObject.FindGameObjectWithTag("Room");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothCameraFollow>();
        player = GameObject.FindGameObjectWithTag("Player");
        
        StartCoroutine(LoadAllRooms());
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
        // default to basic room if no rooms (fallback)
        GameObject Room = Instantiate(baseRoomPrefab);
        // Choose a random room. If a room has been picked in the last 3 rooms, it cannot be picked
        if (roomPool.Count > 0)
        {
            int roomIndex = Random.Range(0, roomPool.Count);
            RoomData roomMap = roomPool[roomIndex];
            List<RoomObjectData> triggerList = new List<RoomObjectData>();

            // Ban this room for the next 3 rooms, readd the third previous room to the pool
            if (roomList.Count > 3)
            {
                previousRooms.Add(roomPool[roomIndex]);
                roomPool.Remove(roomPool[roomIndex]);
            }
            if (previousRooms.Count > 3)
            {
                roomPool.Add(previousRooms[0]);
                previousRooms.Remove(previousRooms[0]);
            }
            if (roomMap.directional)
            {
                Room.GetComponent<RoomObject>().directional = true;
            }
            if (roomMap.random_flip)
            {
                Room.GetComponent<RoomObject>().randomFlip = true;
            }
            if (roomMap.random_rotate)
            {
                Room.GetComponent<RoomObject>().randomRotate = true;
            }

            // Add objects from JSON data
            foreach(RoomObjectData roomObject in roomMap.objects)
            {   
                // Save unlockers for after their triggerables have been instantiated
                if (roomObject.main_unlocker)
                {
                    triggerList.Add(roomObject);
                }
                else
                {
                    Vector2 objectVector = new Vector2(roomObject.x, roomObject.y);
                    GameObject newObject = Instantiate(GetPrefabByType(roomObject.type), Room.transform);
                    newObject.transform.position = objectVector;
                    newObject.transform.rotation = Quaternion.Euler(0f, 0f, roomObject.rotation);

                    newObject.name = roomObject.name;
                }

            }
            // Instantiate unlockers now that their triggerables exist and can be mapped
            if (triggerList.Count > 0)
            {
                foreach(RoomObjectData trigger in triggerList)
                {
                    Vector2 objectVector = new Vector2(trigger.x, trigger.y);
                    GameObject newObject = Instantiate(GetPrefabByType(trigger.type), Room.transform);
                    newObject.transform.position = objectVector;
                    newObject.transform.rotation = Quaternion.Euler(0f, 0f, trigger.rotation);

                    newObject.name = trigger.name;
                    newObject.GetComponent<UnlockerBehavior>().unlockerObjects.Add(newObject);

                    newObject.GetComponent<UnlockerBehavior>().triggerableObject = newObject.transform.parent.Find(trigger.target).gameObject;
                    if (trigger.unlockers.Count > 0)
                    {
                        foreach(string unlocker in trigger.unlockers)
                        {
                            newObject.GetComponent<UnlockerBehavior>().unlockerObjects.Add(newObject.transform.parent.Find(unlocker).gameObject);
                        }
                    }
                }
            }
        }
        return Room;
    }

    public GameObject GetPrefabByType(string typeName)
    {
        foreach(RoomPrefabMapping mapping in prefabMappings)
        {
            if (mapping.key == typeName)
            {
                return mapping.prefab;
            }
        }
        Debug.LogWarning($"Warning: No prefab mapped for key: [{typeName}]");
        return null;
    }
    IEnumerator LoadAllRooms()
    {
        string indexPath = $"{Application.streamingAssetsPath}/Rooms/rooms_index.json";
        
        using (UnityWebRequest request = UnityWebRequest.Get(indexPath))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to load room index: {request.error}");
                yield break;
            }

            string rawIndexText = request.downloadHandler.text;
            
            IndexData indexData = JsonUtility.FromJson<IndexData>(rawIndexText);
            foreach (string fileName in indexData.filenames)
            {
                string roomPath = $"{Application.streamingAssetsPath}/Rooms/{fileName}";
                
                using (UnityWebRequest roomRequest = UnityWebRequest.Get(roomPath))
                {
                    yield return roomRequest.SendWebRequest();

                    if (roomRequest.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogWarning($"Skipped room file '{fileName}': {roomRequest.error}");
                        continue;
                    }

                    string rawRoomText = roomRequest.downloadHandler.text;
                    RoomData roomData = JsonUtility.FromJson<RoomData>(rawRoomText);

                    roomPool.Add(roomData);
                    roomList.Add(roomData);
                }
            }
        }
    }
}

[System.Serializable]
public struct RoomPrefabMapping
{
    public string key;
    public GameObject prefab;
}

[System.Serializable]
public class IndexData
{
    public List<string> filenames;
}
