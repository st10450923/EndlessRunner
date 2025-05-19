using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Vector3 Offset;
    public Transform Player;
    void Start()
    {
        Player = GameObject.FindFirstObjectByType<PlayerControls>().transform;
        Offset = transform.position - Player.position;
    }
    void Update()
    {
        Vector3 Destination = Player.position + Offset;
        Destination.x = 4;
        transform.position = Destination;
    }
}
