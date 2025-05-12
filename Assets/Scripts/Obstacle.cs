using UnityEngine;

public class Obstacle : MonoBehaviour
{

    PlayerControls playerControls;
    void Start()
    {
        playerControls = GameObject.FindFirstObjectByType<PlayerControls>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == playerControls)
        {
            if (playerControls.isShielded == true)
            {
                Destroy(gameObject);
                playerControls.isShielded = false;
            }
            else
                playerControls.KillPlayer();
        }
        
    }
}
