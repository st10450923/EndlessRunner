using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    public float SpinSpeed = 90f;
    public int Duration = 5;

    private void Update()
    {
        transform.Rotate(0, -SpinSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
            return;
        }
        if (other.CompareTag("Player"))
        {
            EventManager.Inst?.TriggerDoubleJumpPickup(Duration);
            SFXManager.Inst?.PlaySFX(SFXManager.Inst.PlayerPickup, 0.5f);
            Destroy(gameObject);
        }
    }
}
