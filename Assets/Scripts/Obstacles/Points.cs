using UnityEngine;

public class Points : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        EventManager.Inst.TriggerPointsGained(1);
        Destroy(gameObject);
    }
}
