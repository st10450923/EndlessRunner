using UnityEngine;

public class SpearController : MonoBehaviour
{
    public float dropSpeed = 15f;         
    public float destroyDepth = -50f;
    public LayerMask groundMask;
    bool Falling = true;

    void Update()
    {
        if(Falling)
            transform.Translate(Vector3.down * dropSpeed * Time.deltaTime, Space.World);

        if (Physics.Raycast(transform.position, Vector3.down, 0.1f, groundMask))
        {
            Falling = false;
        }
        else if (transform.position.y <= destroyDepth)
        {
            Destroy(gameObject);
        }
    }
}
