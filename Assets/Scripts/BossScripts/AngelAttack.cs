using System.Collections;
using UnityEngine;

public class AngelAttack : MonoBehaviour
{
    public GameObject Spear;
    public Transform PlayerTrans; 
    public float SpawnDistanceAhead = 30f;
    public float SpawnHeight =10;
    public float NumberOfSpears =2;
    public float SpearSpread=5f;
    public float SpawnInterval = 3f; 
    private static float TIMER = 0f;

    private void Start()
    {
        PlayerTrans = GameObject.FindFirstObjectByType<PlayerControls>().transform;
        SFXManager.Inst?.PlaySFX(SFXManager.Inst.BossSpawn);
    }
    private void FixedUpdate()
    {
        TIMER += Time.fixedDeltaTime;

        if (TIMER >= SpawnInterval)
        {
            ThrowSpears();
            TIMER = 0f;
        }
    }
    private void ThrowSpears()
    {
        for (int i = 0; i < NumberOfSpears; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-SpearSpread, SpearSpread),SpawnHeight,PlayerTrans.position.z+SpawnDistanceAhead);
            Instantiate(Spear, spawnPos, Quaternion.identity);
        }
        SFXManager.Inst?.PlaySFX(SFXManager.Inst.SpearThrowSound);
    }
}
