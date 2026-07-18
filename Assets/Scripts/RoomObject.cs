using UnityEngine;

public class RoomObject : MonoBehaviour
{
    [SerializeField] public bool directional;
    [SerializeField] public bool randomFlip;
    [SerializeField] public bool randomRotate;
    private bool flip;

    public void Initialize(GameObject previousRoom)
    {
        // Room creation logic, rotation so that if the player can only enter a room from a certain side or has random rotation, it's rotated
        // or if the room can be randomly flipped it can be
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
        }
        else if (randomRotate)
        {
            int rotate = Random.Range(0, 4);
            if (rotate == 1)
            {
                transform.eulerAngles = new Vector3(0, 0, 90);
            }
            else if (rotate == 2)
            {
                transform.eulerAngles = new Vector3(0, 0, -90);
            }
            else if (rotate == 3)
            {
                transform.eulerAngles = new Vector3(0, 0, 180);
            }
        }
        if (randomFlip)
        {
            flip = Random.Range(0, 2) == 0; // '== 0' turns the int into a boolean
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
