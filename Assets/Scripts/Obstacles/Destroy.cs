using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float Delay;
    void Start()
    {
        Destroy(gameObject, Delay);
    }
}
