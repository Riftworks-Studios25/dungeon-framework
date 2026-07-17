using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] public float spinAmount;
    void Update()
    {
        // spin to win
        transform.Rotate(0, 0, spinAmount);
    }
}
