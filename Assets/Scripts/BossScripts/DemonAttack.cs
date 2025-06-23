using UnityEngine;

public class DemonAttack : MonoBehaviour
{
    public GameObject Spear;
    public Transform PlayerTrans; 
    public float SpawnDistanceAhead = 20f;
    public float SpawnInterval = 4f; 
    private static float TIMER = 0f;

    private void Start()
    {
        PlayerTrans = GameObject.FindFirstObjectByType<PlayerControls>().transform;

    }
    private void FixedUpdate()
    {
        TIMER += Time.fixedDeltaTime;

        if (TIMER >= SpawnInterval)
        {
            SpawnSpear();
            TIMER = 0f;
        }
    }

    private void SpawnSpear()
    {
        Vector3 spawnPos = PlayerTrans.position + Vector3.forward * SpawnDistanceAhead;
        spawnPos.y = 0f; 
        Instantiate(Spear, spawnPos, Quaternion.identity);
        Debug.Log("Demon Attacked!");
    }
}
