using UnityEngine;

public class SpinY : MonoBehaviour
{
    [SerializeField] public float spinAmount;
    void Update()
    {
        // spyn to wyn
        transform.Rotate(0, spinAmount, 0);
    }
}
