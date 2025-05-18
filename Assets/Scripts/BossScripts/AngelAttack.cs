using UnityEngine;

public class AngelAttack : MonoBehaviour
{
    public GameObject Beam;
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
            SpawnColumn();
            TIMER = 0f;
        }
    }

    private void SpawnColumn()
    {
        Vector3 spawnPos = PlayerTrans.position + Vector3.forward * SpawnDistanceAhead;
        spawnPos.y = 0f; 
        Instantiate(Beam, spawnPos, Quaternion.identity);
        Debug.Log("Angel Attacked!");
    }
}
