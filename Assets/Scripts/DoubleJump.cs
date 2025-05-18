using Unity.VisualScripting;
using UnityEngine;

public class DoubleJump
    : MonoBehaviour
{
    PlayerControls playerControls;
    public float Spin=90f;
    public int DoubleJumpDuration=5;
    void Start()
    {
        playerControls = GameObject.FindFirstObjectByType<PlayerControls>();
    }

    void Update()
    {
        transform.Rotate(0, -Spin * Time.deltaTime, 0 );
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Obstacle>() != null)
        {
            Destroy(gameObject);
            return;
        }
        if (other.gameObject.name == "Player")
        {
            playerControls.DoubleJump(DoubleJumpDuration);
            Destroy(gameObject);
        }

    }
}
