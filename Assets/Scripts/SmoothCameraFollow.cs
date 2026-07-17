using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float dampening;
    public Transform target;
    private Vector3 vel = Vector3.zero;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Room").transform;
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        targetPosition.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref vel, dampening);
    }

    public void UpdateTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
