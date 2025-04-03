using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform Player;
    Vector3 CameraOffset;
    void Start()
    {
        CameraOffset = transform.position - Player.position;
    }
    void Update()
    {
        Vector3 Target = Player.position + CameraOffset;
        Target.x = 0;
        transform.position = Target;
    }
}
