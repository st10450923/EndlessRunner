using System.Collections;
using UnityEngine;

public enum Zone
{
    Heaven,
    Hell
}
public class ZoneManager : MonoBehaviour
{
    //VariablesForZones
    private Zone CurrentZone;
    public GameObject HeavenShader;
    public GameObject HellShader;
    //Variables for boss
    public GameEngine gameEngine;
    public Transform Player;
    public GameObject Angel;
    public GameObject Demon;
    private GameObject BossInstance;
    public int BossPointValue = 100;
    public float InitialSpawnDelay = 40f;
    public float BossSpawnDistance = 200f;
    private Vector3 BossOffset = new(-10, 2, 5);
    bool BossActive = false;
    private float DistanceSinceBoss = 0;
    public float BossDuration = 20f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameEngine = GameObject.FindFirstObjectByType<GameEngine>();
        Player = GameObject.FindFirstObjectByType<PlayerControls>().transform;
        CurrentZone = Zone.Heaven;
        SpawnBoss(GetBoss(), InitialSpawnDelay);
    }
    void FixedUpdate()
    {
        if (!BossActive)
        {
            DistanceSinceBoss += Player.GetComponent<PlayerControls>().ForwardSpeed * Time.deltaTime;
            if (DistanceSinceBoss >= BossSpawnDistance)
            {
                SpawnBoss(GetBoss());
            }
        }
    }
    public GameObject GetBoss()
    {
        switch (CurrentZone)
        {
            case Zone.Heaven:
                return(Angel);
            case Zone.Hell:
                return(Demon);
            default:
                return(Angel);
        }
    }

    public void SwitchZones()
    {
        if (CurrentZone == Zone.Heaven)
            EnterHeaven();
        else if (CurrentZone == Zone.Hell)
            EnterHell();
    }
    public void EnterHeaven()
    {
        CurrentZone = Zone.Heaven;
        HeavenShader.SetActive(true);
        HellShader.SetActive(false);
    }

    public void EnterHell()
    {
        CurrentZone = Zone.Hell;
        HeavenShader.SetActive(false);
        HellShader.SetActive(true);
    }

    void SpawnBoss(GameObject Boss)
    {
        Vector3 BossPosition = Player.position + BossOffset;
        Quaternion BossRotaiton = Quaternion.Euler(50f, -14f, 0f);
        BossInstance = Instantiate(Boss, BossPosition, BossRotaiton);
        Invoke("DestroyBoss", BossDuration);
        BossActive = true;
        DistanceSinceBoss = 0f;
    }
    // Spawn after a delay
    public void SpawnBoss(GameObject Boss, float delay)
    {
        StartCoroutine(SpawnAfterDelay(Boss, delay));
    }

    private IEnumerator SpawnAfterDelay(GameObject Boss, float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnBoss(Boss);
    }
    void DestroyBoss()
    {
        Debug.Log("Destroyed Boss!");
        Destroy(BossInstance);
        BossActive = false;
        gameEngine.AddPoints(BossPointValue);
    }
}
