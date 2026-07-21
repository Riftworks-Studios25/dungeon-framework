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
    List<RoomData> commonRooms = new List<RoomData>();
    List<RoomData> uncommonRooms = new List<RoomData>();
    List<RoomData> rareRooms = new List<RoomData>();
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

        float roomChance = Random.Range(0f, 1f);
        bool noRooms = false;
        RoomData roomMap = null;

        if (roomChance < 0.65f && commonRooms.Count > 0)
        {
            roomMap = PickRoom(commonRooms);
        } 
        else if (roomChance < 0.90f && uncommonRooms.Count > 0)
        {
            roomMap = PickRoom(uncommonRooms);
        } 
        else if (rareRooms.Count > 0)
        {
            roomMap = PickRoom(rareRooms);
        }
        else
        {
            if (commonRooms.Count > 0)
            {
                roomMap = PickRoom(commonRooms);
            }
            else if (uncommonRooms.Count > 0)
            {
                roomMap = PickRoom(uncommonRooms);
            }
            else
            {
               noRooms = true; 
            }
        }
        Debug.Log(roomChance);

        // Choose a random room. If a room has been picked in the last 3 rooms, it cannot be picked
        if (roomList.Count > 0 && !noRooms)
        {

            // Ban this room for the next 3 rooms, readd the third previous room to the pool
            if (previousRooms.Count > 3)
            {
                switch (previousRooms[0].rarity)
                {
                    case "common":
                            commonRooms.Add(previousRooms[0]);
                            break;
                        case "uncommon":
                            uncommonRooms.Add(previousRooms[0]);
                            break;
                        case "rare":
                            rareRooms.Add(previousRooms[0]);
                            break;
                        default:
                            commonRooms.Add(previousRooms[0]);
                            break;
                }
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
            Room.GetComponent<RoomObject>().rarity = roomMap.rarity;
            
            List<string> triggerableNames = new List<string>();
            List<(GameObject, RoomObjectData)> instantiatedDependents = new List<(GameObject, RoomObjectData)>();
            // Add objects from JSON data
            foreach(RoomObjectData roomObject in roomMap.objects)
            {   
                bool spawn = true;
                if (roomObject.spawn_chance < 1.0f && !roomObject.main_unlocker && !roomObject.dependent)
                {
                    float objectChance = Random.Range(0f, 1f);
                    if (objectChance >= roomObject.spawn_chance)
                    {
                        spawn = false;
                    }
                }
                
                if (spawn)
                {
                    Vector2 objectVector = new Vector2(roomObject.x, roomObject.y);
                    GameObject newObject = Instantiate(GetPrefabByType(roomObject.type), Room.transform);
                    newObject.transform.position = objectVector;
                    newObject.transform.rotation = Quaternion.Euler(0f, 0f, roomObject.rotation);
                    newObject.transform.localScale = new Vector3(roomObject.scale_x, roomObject.scale_y, 1);

                    newObject.name = roomObject.name;

                    if (roomObject.main_unlocker || roomObject.dependent)
                    {
                        instantiatedDependents.Add((newObject, roomObject));
                    }
                }

            }
            foreach(var (triggerObject, triggerData) in instantiatedDependents)
            {
                UnlockerBehavior behavior = null;
                if (triggerObject.TryGetComponent<UnlockerBehavior>(out var bh))
                {
                    behavior = bh;
                    behavior.unlockerObjects.Add(triggerObject);
                }

                Transform targetTransform = Room.transform.Find(triggerData.target);
                if (targetTransform != null)
                {
                    if (behavior != null)
                    {
                        behavior.triggerableObject = targetTransform.gameObject;

                        foreach(string unlocker in triggerData.unlockers)
                        {
                            Transform otherUnlocker = Room.transform.Find(unlocker);
                            if (otherUnlocker != null)
                            {
                                behavior.unlockerObjects.Add(otherUnlocker.gameObject);
                            }
                        }
                    }
                }
                else
                {
                    foreach(string unlocker in triggerData.unlockers)
                    {
                        Transform otherUnlocker = Room.transform.Find(unlocker);
                        if (otherUnlocker != null)
                        {
                            Destroy(otherUnlocker.gameObject);
                        }
                    }
                    Destroy(triggerObject);
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

                    roomList.Add(roomData);
                    switch (roomData.rarity)
                    {
                        case "common":
                            commonRooms.Add(roomData);
                            break;
                        case "uncommon":
                            uncommonRooms.Add(roomData);
                            break;
                        case "rare":
                            rareRooms.Add(roomData);
                            break;
                        default:
                            commonRooms.Add(roomData);
                            Debug.LogWarning($"Warning: Invalid rarity '{roomData.rarity}' in room '{roomData.room_name}'");
                            break;
                    }
                }
            }
        }
    }
    public RoomData PickRoom(List<RoomData> rooms)
    {
        int roomIndex = Random.Range(0, rooms.Count);
        RoomData roomMap = rooms[roomIndex];
        if (roomList.Count > 3)
        {
            previousRooms.Add(rooms[roomIndex]);
            rooms.Remove(rooms[roomIndex]);
        }
        return roomMap;
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
