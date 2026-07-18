using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RoomData
{
    public string room_name;
    public bool directional;
    public bool random_flip;
    public List<RoomObjectData> objects;
}

[System.Serializable]
public class RoomObjectData
{
    public string type;
    public float x;
    public float y;
    public string name;
    public bool main_unlocker;
    public List<string> unlockers;
    public string target;
    public int rotation;
}
