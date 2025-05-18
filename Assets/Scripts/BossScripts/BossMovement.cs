using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public Vector3 Offset;
    private Transform Player;
    void Start()
    {
        Offset = new Vector3(-10,2,5);
        Player = GameObject.FindFirstObjectByType<PlayerControls>().transform;
    }
    void Update()
    {
        Vector3 Destination = Player.position + Offset;
        Destination.x = -10;
        transform.position = Destination;
    }
}
