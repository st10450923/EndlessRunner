using Unity.VisualScripting;
using UnityEngine;

public class PointBoost : MonoBehaviour
{
    PlayerControls playerControls;
    public float Spin=90f;
    public int Duration=10;
    public float BoostMultiplier=1.5f;
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
            playerControls.PointBoost(Duration, BoostMultiplier);
            Destroy(gameObject);
        }

    }
}
