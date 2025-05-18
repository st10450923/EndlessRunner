using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public Transform Target;
    public Vector3 Offset;
    void Start()
    {
        Offset = transform.position - Target.position;
    }
    void Update()
    {
        Vector3 Destination = Target.position + Offset;
        Destination.x = transform.position.x;
        transform.position = Destination;
    }
}
