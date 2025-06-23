using UnityEngine;

public class ShieldBuff : MonoBehaviour
{
    public float Spin = 90f;
    public int Duration = 10;

    void Update()
    {
        transform.Rotate(0, -Spin * Time.deltaTime, 0);
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
            EventManager.Inst?.TriggerShieldPickup(Duration);

            SFXManager.Inst?.PlaySFX(SFXManager.Inst.PlayerPickup, 0.5f);
            Destroy(gameObject);
        }
    }
}
