using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RoomData
{
    public string room_name;
    public bool directional;
    public bool random_flip;
    public bool random_rotate;
    public string rarity = "common";
    public List<RoomObjectData> objects;
}

[System.Serializable]
public class RoomObjectData
{
    public string type;
    public float x;
    public float y;
    public float scale_x = 1.0f;
    public float scale_y = 1.0f;
    public string name;
    public bool main_unlocker;
    public bool dependent;
    public List<string> unlockers;
    public string target;
    public int rotation;
    public float spawn_chance = 1.0f;
    public bool rotate_fix = true;
}
