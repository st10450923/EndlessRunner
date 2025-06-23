using UnityEngine;

public class DemonAttack : MonoBehaviour
{
    public GameObject Beam;
    public Transform PlayerTrans; 
    public float SpawnDistanceAhead = 10f;
    public float SpawnInterval = 4f; 
    private static float TIMER = 0f;

    private void Start()
    {
        PlayerTrans = GameObject.FindFirstObjectByType<PlayerControls>().transform;
        SFXManager.Inst?.PlaySFX(SFXManager.Inst.DemonSpawn);
    }
    private void FixedUpdate()
    {
        TIMER += Time.fixedDeltaTime;

        if (TIMER >= SpawnInterval)
        {
            SpawnBeam();
            TIMER = 0f;
        }
    }

    private void SpawnBeam()
    {
        Vector3 spawnPos = PlayerTrans.position + Vector3.forward * SpawnDistanceAhead;
        spawnPos.y = 0f; 
        Instantiate(Beam, spawnPos, Quaternion.identity);
        SFXManager.Inst.PlaySFX(SFXManager.Inst.BossAttack);
    }
}
