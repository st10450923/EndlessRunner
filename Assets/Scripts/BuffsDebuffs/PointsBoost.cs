using UnityEngine;

public class PointBoost : MonoBehaviour
{
    public float Spin = 90f;
    public int Duration = 10;
    public float BoostMultiplier = 1.5f;

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
            EventManager.Inst?.TriggerPointsMultiplierPickup(Duration, BoostMultiplier);
            SFXManager.Inst?.PlaySFX(SFXManager.Inst.PlayerPickup);
            Destroy(gameObject);
        }
    }
}
