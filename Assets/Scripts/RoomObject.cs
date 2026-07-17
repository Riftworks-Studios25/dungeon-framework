using UnityEngine;

public class RoomObject : MonoBehaviour
{
    [SerializeField] public bool directional;
    [SerializeField] public bool randomFlip;
    private bool flip;

    public void Initialize(GameObject previousRoom)
    {
        // Room creation logic, rotation so that if the player can only enter a room from a certain side, it's rotated 
        // or if the room can be randomly flipped 180 degrees it can be
        if (directional)
        {
            float xDiff = previousRoom.transform.position.x - transform.position.x;
            float yDiff = previousRoom.transform.position.y - transform.position.y;
            if (xDiff > 1)
            {
                transform.eulerAngles = new Vector3(0, 0, 90);
            }
            else if (xDiff < -1)
            {
                 transform.eulerAngles = new Vector3(0, 0, -90);
            }
            if (yDiff > 1)
            {
                transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else if (yDiff < -1)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        if (randomFlip)
        {
            flip = Random.Range(0, 2) == 0;
            if (flip)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        SetupTriggers();
    }

    private void SetupTriggers()
    {
        // Create triggers after rotating so that they are independent of rotations
        GameObject Triggers = Resources.Load("Prefabs/Triggers") as GameObject;
        GameObject TriggersObject = (GameObject) Instantiate(Triggers, new Vector2(transform.position.x, transform.position.y), Quaternion.identity, parent: this.transform);
        foreach (Transform childTransform in TriggersObject.transform)
        {
        TriggerCollide triggerCollide = childTransform.GetComponent<TriggerCollide>();
        triggerCollide.SetParent(gameObject);
        if (flip)
        {
            TriggersObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        }
    }
}
