using UnityEngine;

public class Points : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "Player")
        {
            return;
        }
        GameEngine.Inst.IncrementScore();
        Destroy(gameObject);
    }
}
