using Unity.VisualScripting;
using UnityEngine;

public class ShieldBuff : MonoBehaviour
{
    PlayerControls playerControls;
    public float Spin=90f;
    public int Duration=5;
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
            playerControls.ShieldBuff(Duration);
            Destroy(gameObject);
        }

    }
}
